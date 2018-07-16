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
        /// Locate migrations in `default` location. For <see cref="DefaultClickHouseMigrationLocator"/> it is Assembly.GetEntryAssembly()
        /// </summary>
        /// <returns></returns>
        IOrderedEnumerable<Migration> Locate();

        /// <summary>
        /// Locate migrations in specified assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IOrderedEnumerable<Migration> Locate(Assembly assembly);
    }
}