using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Utility extensions for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Get the last item in a collection that may be empty or NULL.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="items">The collection.</param>
        /// <returns>The last item in the collection, or the default value if empty or NULL.</returns>
        public static T LastOrDefaultSafe<T>(this T[] items)
        {
            return items != null && items.Length > 0 ? items.Last() : default;
        }

        /// <summary>
        /// Update a non-generic list to reflect a given collection of items.
        /// </summary>
        /// <remarks>
        /// Items are removed and added where needed without clearing the list first.
        /// The order of items in <paramref name="list"/> may differ from <paramref name="items"/>.
        /// This method is useful for non-generic lists, like some in user interfaces.
        /// </remarks>
        /// <typeparam name="T">Type of items in the list.</typeparam>
        /// <param name="list">The list to be updated.</param>
        /// <param name="items">The desired state for the list.</param>
        public static void Update<T>(this IList list, IReadOnlyCollection<T> items)
        {
            var current = list.OfType<T>().ToArray();
            var add = items.Except(current);
            var remove = current.Except(items);

            foreach (var item in remove)
            {
                list.Remove(item);
            }

            foreach (var item in add)
            {
                list.Add(item);
            }
        }
    }
}