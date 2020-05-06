using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Simple independent selection list that can be manipulated in code.
    /// </summary>
    /// <typeparam name="T">Type of elements in the selection.</typeparam>
    public class SelectionList<T> : ISelectionList<T>
    {
        private readonly SourceList<T> _list = new SourceList<T>();
        private readonly Subject<Unit> _refresh = new Subject<Unit>();

        /// <inheritdoc/>
        public int Count => _list.Count;

        /// <inheritdoc/>
        public IObservable<int> CountChanged => _list.CountChanged;

        /// <inheritdoc/>
        public IEnumerable<T> Items => _list.Items;

        /// <inheritdoc/>
        public IObservable<Unit> OnRefresh => _refresh.AsObservable();

        /// <summary>
        /// Add an item to the selection.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        public void Add(T item)
        {
            if (_list.Items.Contains(item))
            {
                return;
            }

            _list.Add(item);
        }

        /// <inheritdoc/>
        public IObservable<IChangeSet<T>> Connect(Func<T, bool> predicate = null)
        {
            return _list.Connect(predicate);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _list.Dispose();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.Items.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.Items.GetEnumerator();
        }

        /// <inheritdoc/>
        public IObservable<IChangeSet<T>> Preview(Func<T, bool> predicate = null)
        {
            return _list.Preview(predicate);
        }

        /// <inheritdoc/>
        public void Refresh()
        {
            _refresh.OnNext(Unit.Default);
        }

        /// <summary>
        /// Remove an item from the selection.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns>Value indicating whether the item could be removed.</returns>
        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        /// <inheritdoc/>
        public void Update(params T[] items)
        {
            _list.EditDiffOrdered(items);
        }
    }
}