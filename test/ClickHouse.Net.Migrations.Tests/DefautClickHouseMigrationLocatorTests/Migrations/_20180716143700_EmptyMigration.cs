namespace ClickHouse.Net.Migrations.Tests.DefautClickHouseMigrationLocatorTests.Migrations
{
    public class _20180716143700_EmptyMigration : Migration
    {
        public override bool Process(IClickHouseDatabase provider)
        {
            return true;
        }
    }
}
