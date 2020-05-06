using System;
using System.Linq;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list base class for user-interface binding to controls that
    /// only support selection changes through <see cref="Add(T)"/> and <see cref="Remove(T)"/> operations.
    /// </summary>
    /// <typeparam name="T">Type of element in the list.</typeparam>
    public abstract class SelectionListAddRemove<T> : SelectionListAbstract<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionListAddRemove{T}"/> class.
        /// </summary>
        /// <param name="changes">Source stream of selection changes.</param>
        public SelectionListAddRemove(IObservable<IChangeSet<T>> changes)
            : base(changes)
        {
        }

        /// <inheritdoc/>
        public override void Update(params T[] items)
        {
            var remove = Items.Except(items).ToArray();
            foreach (var item in remove)
            {
                Remove(item);
            }

            var add = items.Except(Items).ToArray();
            foreach (var item in add)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Add an item to the selection of the bound control.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        protected abstract void Add(T item);

        /// <summary>
        /// Remove an item to the selection of the bound control.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        protected abstract void Remove(T item);
    }
}