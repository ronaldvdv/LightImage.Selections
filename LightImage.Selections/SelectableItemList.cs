using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list representing the selected <see cref="ISelectable"/> items in a collection.
    /// </summary>
    /// <typeparam name="T">Type of selected items.</typeparam>
    public class SelectableItemList<T> : SelectionListAddRemove<T>
        where T : ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableItemList{T}"/> class.
        /// </summary>
        /// <param name="source">Observable collection containing the <see cref="ISelectable"/> items.</param>
        public SelectableItemList(ReadOnlyObservableCollection<T> source)
            : base(GetSelectedItems(source))
        {
        }

        /// <inheritdoc/>
        protected override void Add(T item)
        {
            item.IsSelected = true;
        }

        /// <inheritdoc/>
        protected override void Remove(T item)
        {
            item.IsSelected = false;
        }

        private static IObservable<IChangeSet<T>> GetSelectedItems(ReadOnlyObservableCollection<T> source)
        {
            return source.ToObservableChangeSet().AutoRefreshOnObservable(a => a.WhenAnyValue(x => x.IsSelected)).Filter(a => a.IsSelected);
        }
    }
}