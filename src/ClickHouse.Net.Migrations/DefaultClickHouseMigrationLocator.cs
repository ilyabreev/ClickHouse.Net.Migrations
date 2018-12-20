using System;
using System.Linq;
using System.Reflection;

namespace ClickHouse.Net.Migrations
{
    public class DefaultClickHouseMigrationLocator : IClickHouseMigrationLocator
    {
        public IOrderedEnumerable<Migration> Locate(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly
                .GetTypes()
                .Where(t => typeof(Migration).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (Migration) Activator.CreateInstance(t))
                .OrderBy(m => m.CreatedAt);
        }
    }
}
