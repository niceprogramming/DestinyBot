using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new Random();

        //https://stackoverflow.com/questions/3453274/using-linq-to-get-the-last-n-elements-of-a-collection
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            var enumerable = source.ToList();
            return enumerable.Skip(Math.Max(0, enumerable.Count() - n));
        }


        public static T Random<T>(this IList<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (items.Count == 0)
            {
                return default;
            }

            var index = _random.Next(0, items.Count);
            var item = items[index];

            return item;
        }
    }
}
