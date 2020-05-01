using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace LightImage.Selections
{
    public abstract class SelectionListAbstract<T> : ISelectionList<T>
    {
        private readonly IObservableList<T> _list;

        public SelectionListAbstract(IObservable<IChangeSet<T>> changes)
        {
            _list = changes.AsObservableList();
        }

        public int Count => _list.Count;
        public IObservable<int> CountChanged => _list.CountChanged;

        public IEnumerable<T> Items => _list.Items;

        public IObservable<Unit> OnRefresh => Observable.Never<Unit>();

        public IObservable<IChangeSet<T>> Connect(Func<T, bool> predicate = null)
        {
            return _list.Connect(predicate);
        }

        public void Dispose()
        {
            _list.Dispose();
        }

        public IEnumerator<T> GetEnumerator() => _list.Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.Items.GetEnumerator();

        public IObservable<IChangeSet<T>> Preview(Func<T, bool> predicate = null)
        {
            return _list.Preview(predicate);
        }

        public void Refresh()
        {
            // Ignore 'Refresh' operations
        }

        public abstract void Update(params T[] items);
    }
}