using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Base class for selection lists.
    /// </summary>
    /// <typeparam name="T">Type of item in the selection.</typeparam>
    public abstract class SelectionListAbstract<T> : ISelectionList<T>
    {
        private readonly IObservableList<T> _list;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionListAbstract{T}"/> class.
        /// </summary>
        /// <param name="changes">Source stream of selection changes.</param>
        public SelectionListAbstract(IObservable<IChangeSet<T>> changes)
        {
            _list = changes.AsObservableList();
        }

        /// <inheritdoc/>
        public int Count => _list.Count;

        /// <inheritdoc/>
        public IObservable<int> CountChanged => _list.CountChanged;

        /// <inheritdoc/>
        public IEnumerable<T> Items => _list.Items;

        /// <inheritdoc/>
        public IObservable<Unit> OnRefresh => Observable.Never<Unit>();

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
            // Ignore 'Refresh' operations
        }

        /// <inheritdoc/>
        public abstract void Update(params T[] items);
    }
}