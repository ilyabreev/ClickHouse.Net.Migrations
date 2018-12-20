using System.Reflection;

namespace ClickHouse.Net.Migrations
{
    /// <summary>
    /// Service for managing ClickHouse migrations
    /// </summary>
    public interface IClickHouseMigrations
    {
        /// <summary>
        /// Apply all migrations not yet applied
        /// </summary>
        void ApplyMigrations(bool createDatabaseIfNotExists = true);

        /// <summary>
        /// Apply all migrations not yed applied and located in specific assembly
        /// </summary>
        /// <param name="assembly">Assembly to search migrations for</param>
        /// <param name="createDatabaseIfNotExists">Create a new database if it not already exists</param>
        void ApplyMigrations(Assembly assembly, bool createDatabaseIfNotExists = true);
    }
}