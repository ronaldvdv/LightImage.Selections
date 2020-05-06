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

    internal static class UtilityExtensions
    {
        public static IChangeSet<T> GetRefreshChangeSet<T>(this IEnumerable<T> items)
        {
            return new ChangeSet<T>(items.Select((item, index) => new Change<T>(ListChangeReason.Refresh, item, index)));
        }

        public static IObservable<IChangeSet<T>> ScanDiff<T>(this IObservable<Unit> events, Func<IEnumerable<T>> getter)
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