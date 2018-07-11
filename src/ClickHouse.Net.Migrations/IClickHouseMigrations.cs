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
    }
}