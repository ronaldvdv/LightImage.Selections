using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using ReactiveUI;

namespace LightImage.Selections
{
    /// <summary>
    /// Extension methods on selection lists and related collections.
    /// </summary>
    public static class SelectionListExtensions
    {
        /// <summary>
        /// Bind the primary selection in a list to some view model property.
        /// </summary>
        /// <typeparam name="TOwner">Type of view model.</typeparam>
        /// <typeparam name="TItem">Type of items in the selection.</typeparam>
        /// <param name="list">Source selection list.</param>
        /// <param name="owner">The target view model.</param>
        /// <param name="property">Property on the view model to which the selection should be bound.</param>
        /// <returns>Disposable for clearing the binding.</returns>
        public static IDisposable BindPrimaryItem<TOwner, TItem>(this ISelectionList<TItem> list, TOwner owner, Expression<Func<TOwner, TItem>> property)
            where TOwner : class
        {
            return ObservePrimarySelection(list).BindTo(owner, property);
        }

        /// <summary>
        /// Transform an observable collection of <see cref="ISelectable"/> items into a selection list.
        /// </summary>
        /// <typeparam name="T">Type of items in the list.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <returns>The selection list that synchronizes with the selected items in the given collection.</returns>
        public static ISelectionList<T> GetSelectionList<T>(this ReadOnlyObservableCollection<T> source)
            where T : ISelectable
        {
            return new SelectableItemList<T>(source);
        }

        /// <summary>
        /// Transform a property on a view model representing a single selected item, into a selection list.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <typeparam name="TItem">Type of selected items.</typeparam>
        /// <param name="vm">The view model.</param>
        /// <param name="member">Expression representing the view model property that represents the selection.</param>
        /// <returns>Selection list representing the item in the view model property.</returns>
        public static ISelectionList<TItem> GetSelectionList<TViewModel, TItem>(this TViewModel vm, Expression<Func<TViewModel, TItem>> member)
        {
            return SingleSelectionList<TItem>.Create(vm, member);
        }

        /// <summary>
        /// Observe a primary item in a selection list. This is either the last selected item,
        /// or the default value for <typeparamref name="TItem"/> if nothing is selected.
        /// </summary>
        /// <typeparam name="TItem">Type of selected item.</typeparam>
        /// <param name="list">The selection list.</param>
        /// <returns>The primary selected item.</returns>
        public static IObservable<TItem> ObservePrimarySelection<TItem>(ISelectionList<TItem> list)
        {
            return list.Connect().Select(_ => list.LastOrDefault());
        }

        /// <summary>
        /// Synchronize two selection lists.
        /// </summary>
        /// <typeparam name="T">Type of item in the selection lists.</typeparam>
        /// <param name="left">First selection list.</param>
        /// <param name="right">Second selection list.</param>
        /// <returns>Disposable for decoupling the synchronization between the lists.</returns>
        public static IDisposable Synchronize<T>(this ISelectionList<T> left, ISelectionList<T> right)
        {
            return new SelectionSynchronizer<T>(left, right);
        }
    }
}