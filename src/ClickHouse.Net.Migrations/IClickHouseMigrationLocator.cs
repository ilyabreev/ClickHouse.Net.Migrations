using System.Linq;
using System.Reflection;

namespace ClickHouse.Net.Migrations
{
    /// <summary>
    /// Service for locating migrations
    /// </summary>
    public interface IClickHouseMigrationLocator
    {
        /// <summary>
        /// Locate migrations in specified assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IOrderedEnumerable<Migration> Locate(Assembly assembly);
    }
}