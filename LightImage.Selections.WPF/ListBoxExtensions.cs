using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Extension methods for the <see cref="ListBox"/> class.
    /// </summary>
    public static class ListBoxExtensions
    {
        /// <summary>
        /// Get a strongly-typed collection of selected items in a <see cref="ListBox"/>,
        /// taking into account the different <see cref="ListBox.SelectionMode"/> options.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="listBox">The listbox holding the selection.</param>
        /// <returns>Collection of selected items.</returns>
        public static IEnumerable<T> GetSelection<T>(this ListBox listBox)
        {
            if (listBox.SelectionMode == SelectionMode.Extended)
            {
                return listBox.SelectedItems.OfType<T>();
            }

            if (listBox.SelectedItem is T item)
            {
                return Enumerable.Repeat(item, 1);
            }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Get a <see cref="ISelectionList{T}"/> that represents the selection in a <see cref="ListBox"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="listBox">The listbox to be represented.</param>
        /// <returns>Selection list representing the selection in the listbox.</returns>
        public static ISelectionList<T> GetSelectionList<T>(this ListBox listBox)
            where T : class
        {
            return new ListBoxSelectionList<T>(listBox);
        }

        /// <summary>
        /// Get an observable stream of changes to the selection in a <see cref="ListBox"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="listBox">The listbox to be observed.</param>
        /// <returns>Observable stream of changes to the selection of the <paramref name="listBox"/>.</returns>
        public static IObservable<IChangeSet<T>> ObserveSelection<T>(this ListBox listBox)
        {
            var events = new RxSelectorEvents(listBox);
            var selChangedEvent = events.SelectionChanged;
            var changes = selChangedEvent.Select(_ => Unit.Default).ScanDiff(() => GetSelection<T>(listBox));
            var focusedEvent = events.GotFocus;
            var refresh = focusedEvent.Select(_ => listBox.GetSelection<T>().GetRefreshChangeSet());
            return refresh.Merge(changes);
        }

        /// <summary>
        /// Synchronize the selection of a <see cref="ListBox"/> with a given other selection list.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="listBox">The listbox to be synchronized.</param>
        /// <param name="list">The other selection list.</param>
        /// <returns>Disposable representing the synchronization binding between <paramref name="list"/> and <paramref name="listBox"/>.</returns>
        public static IDisposable Synchronize<T>(this ListBox listBox, ISelectionList<T> list)
            where T : class
        {
            return list.Synchronize(listBox.GetSelectionList<T>());
        }
    }
}