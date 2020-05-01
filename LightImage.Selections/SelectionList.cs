using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LightImage.Selections
{
    public class SelectionList<T> : ISelectionList<T>
    {
        private SourceList<T> _list = new SourceList<T>();
        private Subject<Unit> _refresh = new Subject<Unit>();

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Refresh()
        {
            _refresh.OnNext(Unit.Default);
        }

        public void Remove(T item)
        {
            _list.Remove(item);
        }

        public void Update(params T[] items)
        {
            _list.EditDiffOrdered(items);
        }

        #region IObservableList<T> implementation

        public int Count => _list.Count;
        public IObservable<int> CountChanged => _list.CountChanged;

        public IEnumerable<T> Items => _list.Items;

        public IObservable<Unit> OnRefresh => _refresh.AsObservable();

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

        #endregion IObservableList<T> implementation
    }
}