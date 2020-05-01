using DynamicData;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;

namespace LightImage.Selections
{
    public static class CollectionExtensions
    {
        public static T LastOrDefaultSafe<T>(this T[] items)
        {
            return items != null && items.Length > 0 ? items.Last() : default;
        }

        public static void Update<T>(this IList list, T[] items)
        {
            var current = list.OfType<T>().ToArray();
            var add = items.Except(current);
            var remove = current.Except(items);

            foreach (var item in remove)
                list.Remove(item);
            foreach (var item in add)
                list.Add(item);
        }
    }
}