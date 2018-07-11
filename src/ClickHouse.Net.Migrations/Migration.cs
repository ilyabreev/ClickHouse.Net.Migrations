using System;
using System.Globalization;

namespace ClickHouse.Net.Migrations
{
    /// <summary>
    /// Base class for all user migrations
    /// </summary>
    public abstract class Migration
    {
        public abstract bool Process(IClickHouseDatabase provider);

        public string Name => GetFromName(1);

        public DateTime CreatedAt => DateTime.ParseExact(GetFromName(0), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        private string GetFromName(int index)
        {
            return GetType().Name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[index];
        }
    }
}