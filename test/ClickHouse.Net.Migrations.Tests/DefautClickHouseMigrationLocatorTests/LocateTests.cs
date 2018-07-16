using System.Linq;
using System.Reflection;
using Xunit;

namespace ClickHouse.Net.Migrations.Tests.DefautClickHouseMigrationLocatorTests
{
    public class LocateTests
    {
        private readonly DefaultClickHouseMigrationLocator _target;

        public LocateTests()
        {
            _target = new DefaultClickHouseMigrationLocator();
        }

        [Fact]
        public void Should_Find_Migrations()
        {
            var result = _target.Locate(Assembly.GetExecutingAssembly()).ToArray();
            Assert.Single(result);
        }
    }
}
