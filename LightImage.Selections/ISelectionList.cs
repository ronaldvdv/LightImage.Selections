using System;
using System.Collections.Generic;
using System.Reactive;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Contract for strongly-typed, observable lists representing a selection of items in the view or view model layer.
    /// </summary>
    /// <typeparam name="T">Type of item in the list.</typeparam>
    public interface ISelectionList<T> : IObservableList<T>, IEnumerable<T>
    {
        /// <summary>
        /// Gets an observable stream of <see cref="ChangeReason.Refresh"/> changes to the list.
        /// </summary>
        IObservable<Unit> OnRefresh { get; }

        /// <summary>
        /// Handles a <see cref="ChangeReason.Refresh"/> change to a synchronized list.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Change the selection to reflect a given desired state.
        /// </summary>
        /// <param name="items">Items to be selected.</param>
        void Update(params T[] items);
    }
}