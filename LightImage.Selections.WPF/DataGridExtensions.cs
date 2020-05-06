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
    /// Extension methods for the <see cref="DataGrid"/> class.
    /// </summary>
    public static class DataGridExtensions
    {
        /// <summary>
        /// Get the collection of selected items in a <see cref="DataGrid"/>,
        /// taking into account the different <see cref="DataGrid.SelectionMode"/> options.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="grid">The datagrid holding the selection.</param>
        /// <returns>Collection of selected items.</returns>
        public static IEnumerable<T> GetSelection<T>(this DataGrid grid)
        {
            if (grid.SelectionMode == DataGridSelectionMode.Extended)
            {
                return grid.SelectedItems.OfType<T>();
            }

            if (grid.SelectedItem is T item)
            {
                return Enumerable.Repeat(item, 1);
            }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Get a <see cref="ISelectionList{T}"/> that represents the selection in a <see cref="DataGrid"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="grid">The datagrid to be represented.</param>
        /// <returns>Selection list representing the selection in the grid.</returns>
        public static ISelectionList<T> GetSelectionList<T>(this DataGrid grid)
            where T : class
        {
            return new DataGridSelectionList<T>(grid);
        }

        /// <summary>
        /// Get an observable stream of changes to the selection in a <see cref="DataGrid"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="grid">The datagrid to be observed.</param>
        /// <returns>Observable stream of changes to the selection of the <paramref name="grid"/>.</returns>
        public static IObservable<IChangeSet<T>> ObserveSelection<T>(this DataGrid grid)
        {
            var events = new RxSelectorEvents(grid);
            var selChangedEvent = events.SelectionChanged;
            var changes = selChangedEvent.Select(_ => Unit.Default).ScanDiff(() => GetSelection<T>(grid));
            var focusedEvent = events.GotFocus;
            var refresh = focusedEvent.Select(_ => grid.GetSelection<T>().GetRefreshChangeSet());
            return refresh.Merge(changes);
        }

        /// <summary>
        /// Synchronize the selection of a <see cref="DataGrid"/> with a given other selection list.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="grid">The datagrid to be synchronized.</param>
        /// <param name="list">The other selection list.</param>
        /// <returns>Disposable representing the synchronization binding between <paramref name="list"/> and <paramref name="grid"/>.</returns>
        public static IDisposable Synchronize<T>(this DataGrid grid, ISelectionList<T> list)
            where T : class
        {
            return list.Synchronize(grid.GetSelectionList<T>());
        }
    }
}