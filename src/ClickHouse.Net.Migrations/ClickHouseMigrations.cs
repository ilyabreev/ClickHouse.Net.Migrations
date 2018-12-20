using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ClickHouse.Ado;
using ClickHouse.Net.Entities;
using Microsoft.Extensions.Logging;

namespace ClickHouse.Net.Migrations
{
    public class ClickHouseMigrations : IClickHouseMigrations
    {
        private const string MigrationsTableName = "applied_migrations";

        private readonly IClickHouseDatabase _dbProvider;
        private readonly ILogger<ClickHouseMigrations> _logger;
        private readonly ClickHouseConnectionSettings _connectionSettings;
        private readonly IClickHouseMigrationLocator _locator;

        public ClickHouseMigrations(
            IClickHouseDatabase dbProvider, 
            ILogger<ClickHouseMigrations> logger, 
            ClickHouseConnectionSettings connectionSettings,
            IClickHouseMigrationLocator locator)
        {
            _dbProvider = dbProvider;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(connectionSettings.Database))
            {
                throw new ArgumentException("Error applying migrations. Database is not set in connectionSettings.", nameof(connectionSettings));
            }

            _connectionSettings = connectionSettings;
            _locator = locator;
        }

        public void ApplyMigrations(bool createDatabaseIfNotExists)
        {
            ApplyMigrations(Assembly.GetEntryAssembly(), createDatabaseIfNotExists);
        }

        public void ApplyMigrations(Assembly assembly, bool createDatabaseIfNotExists)
        {
            if (createDatabaseIfNotExists)
            {
                CreateDatabaseIfNotExists();
            }

            _dbProvider.Open();
            if (!MigrationTableExists())
            {
                CreateMigrationTableIfNotExists();
            }
            
            var migrations = _locator.Locate(assembly);
            var notAppliedMigrations = GetNotAppliedMigration(migrations.ToList(), AllAppliedMigrations());
            var success = ApplyMigrations(notAppliedMigrations);
            if (success)
            {
                _logger.LogInformation("All migrations were successfully applied");
            }
            else
            {
                _logger.LogError("Migrations were not applied");
            }

            _dbProvider.Close();
        }

        private void CreateDatabaseIfNotExists()
        {
            var noDatabaseSettings = GetConnectionSettingsWithoutDatabase(_connectionSettings);
            _dbProvider.ChangeConnectionSettings(noDatabaseSettings);
            _dbProvider.CreateDatabase(_connectionSettings.Database);
            _dbProvider.ChangeConnectionSettings(_connectionSettings);
        }

        private ClickHouseConnectionSettings GetConnectionSettingsWithoutDatabase(ClickHouseConnectionSettings connectionSettings)
        {
            return new ClickHouseConnectionSettings(Regex.Replace(connectionSettings.ToString(), @"Database=[^;]+;", ""));
        }

        private bool ApplyMigration(Migration migration)
        {
            _logger.LogInformation($"Applying {migration.Name}...");

            var success = migration.Process(_dbProvider) && MakeMigrationRecord(migration);
            if (success)
            {
                _logger.LogInformation($"{migration.Name} applied");
            }
            else
            {
                _logger.LogError($"Can't apply {migration.Name}");
            }

            return success;
        }

        private bool ApplyMigrations(IEnumerable<Migration> notAppliedMigrations)
        {
            foreach (var migration in notAppliedMigrations)
            {
                try
                {
                    if (!ApplyMigration(migration))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception was thrown while trying to apply {migration.Name}");
                    return false;
                }
            }

            return true;
        }

        private bool MigrationTableExists()
        {
            return _dbProvider.TableExists(MigrationsTableName);
        }

        private void CreateMigrationTableIfNotExists()
        {
            _dbProvider.CreateTable(new Table
            {
                Name = MigrationsTableName,
                Engine = "TinyLog",
                Columns = new List<Column>
                {
                    new Column("applied_at", "DateTime", "DEFAULT now()"),
                    new Column("name", "String")
                }
            });
        }

        private IEnumerable<string> AllAppliedMigrations()
        {
            var commandText = $"SELECT name FROM {MigrationsTableName}";
            return _dbProvider.ReadAsStringsList(commandText);
        }

        private IEnumerable<Migration> GetNotAppliedMigration(List<Migration> allMigration, IEnumerable<string> appliedMigration)
        {
            foreach (Migration migration in allMigration)
            {
                if (!appliedMigration.Contains(migration.Name))
                {
                    yield return migration;
                }
            }
        }

        private bool MakeMigrationRecord(Migration migration)
        {
            _dbProvider.ExecuteNonQuery($"INSERT INTO {MigrationsTableName} (name) VALUES ('{migration.Name}')");
            return true;
        }
    }
}