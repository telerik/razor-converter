using System;
using System.Collections.Generic;

namespace Telerik.RazorConverter
{
    internal static class Extensions
    {
        public static IList<T> SortByOrder<T>(this IEnumerable<Lazy<T, IOrderMetadata>> registrations)
        {
            var sortedRenderers = new SortedList<int, T>();

            foreach (var reg in registrations)
            {
                sortedRenderers.Add(reg.Metadata.Order, reg.Value);
            }

            return sortedRenderers.Values;
        }
    }

}
