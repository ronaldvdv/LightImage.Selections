using DynamicData;
using LightImage.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LightImage.Selections
{
    public static class SelectionControlExtensions
    {
        public static IDisposable BindSelection<T>(this ListBox listBox, ISelectionList<T> list) where T : class
        {
            return list.Synchronize(listBox.GetSelectionList<T>());
        }

        public static ISelectionList<T> GetSelectionList<T>(this DataGrid grid) where T : class
        {
            return new DataGridSelectionList<T>(grid);
        }

        public static ISelectionList<T> GetSelectionList<T>(this ListBox listBox) where T : class
        {
            return new ListBoxSelectionList<T>(listBox);
        }

        public static IDisposable Synchronize<T>(this DataGrid grid, ISelectionList<T> list) where T : class
        {
            return list.Synchronize(grid.GetSelectionList<T>());
        }

        public static IDisposable Synchronize<T>(this ListBox listBox, ISelectionList<T> list) where T : class
        {
            return list.Synchronize(listBox.GetSelectionList<T>());
        }

        internal static IObservable<IChangeSet<T>> ObserveSelection<T>(this ListBox listBox)
        {
            var events = new RxSelectorEvents(listBox);
            var selChangedEvent = events.SelectionChanged;
            var changes = selChangedEvent.Select(_ => Unit.Default).ScanDiff(() => GetListBoxSelection<T>(listBox));
            var focusedEvent = events.GotFocus;
            var refresh = focusedEvent.Select(_ => GetRefreshChangeSet<T>(GetListBoxSelection<T>(listBox)));
            return refresh.Merge(changes);
        }

        internal static IObservable<IChangeSet<T>> ObserveSelection<T>(this DataGrid grid)
        {
            var events = new RxSelectorEvents(grid);
            var selChangedEvent = events.SelectionChanged;
            var changes = selChangedEvent.Select(_ => Unit.Default).ScanDiff(() => GetDataGridSelection<T>(grid));
            var focusedEvent = events.GotFocus;
            var refresh = focusedEvent.Select(_ => GetRefreshChangeSet<T>(GetDataGridSelection<T>(grid)));
            return refresh.Merge(changes);
        }

        private static IEnumerable<T> GetDataGridSelection<T>(DataGrid grid)
        {
            if (grid.SelectionMode == DataGridSelectionMode.Extended)
                return grid.SelectedItems.OfType<T>();
            if (grid.SelectedItem is T item)
                return Enumerable.Repeat(item, 1);
            return Enumerable.Empty<T>();
        }

        private static IEnumerable<T> GetListBoxSelection<T>(ListBox listBox)
        {
            if (listBox.SelectionMode == SelectionMode.Extended)
                return listBox.SelectedItems.OfType<T>();
            if (listBox.SelectedItem is T item)
                return Enumerable.Repeat(item, 1);
            return Enumerable.Empty<T>();
        }

        private static IChangeSet<T> GetRefreshChangeSet<T>(IEnumerable<T> items)
        {
            return new ChangeSet<T>(items.Select((item, index) => new Change<T>(ListChangeReason.Refresh, item, index)));
        }

        private static IObservable<IChangeSet<T>> ScanDiff<T>(this IObservable<Unit> events, Func<IEnumerable<T>> getter)
        {
            return events.Scan(new ChangeAwareList<T>(), (selected, args) =>
            {
                var current = getter();
                selected.EditDiffOrdered(current);
                return selected;
            }).Select(list => list.CaptureChanges());
        }
    }
}