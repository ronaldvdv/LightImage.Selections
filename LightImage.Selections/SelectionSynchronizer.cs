using System;
using System.Linq;
using System.Reactive.Disposables;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Synchronization between two selection lists.
    /// </summary>
    /// <typeparam name="T">Type of items in each selection list.</typeparam>
    internal class SelectionSynchronizer<T> : IDisposable
    {
        private readonly CompositeDisposable _disposable;
        private bool _locked = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionSynchronizer{T}"/> class.
        /// </summary>
        /// <param name="left">First selection list.</param>
        /// <param name="right">Second selection list.</param>
        public SelectionSynchronizer(ISelectionList<T> left, ISelectionList<T> right)
        {
            _disposable = new CompositeDisposable(
                Bind(left, right),
                Bind(right, left));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private IDisposable Bind(ISelectionList<T> leader, ISelectionList<T> follower)
        {
            return new CompositeDisposable(
                leader.Connect().Subscribe(cs =>
                {
                    if (!_locked)
                    {
                        var hasRefresh = cs.Any(c => c.Reason == ListChangeReason.Refresh);
                        _locked = true;
                        follower.Update(leader.Items.ToArray());
                        if (hasRefresh)
                        {
                            follower.Refresh();
                        }

                        _locked = false;
                    }
                }),
                leader.OnRefresh.Subscribe(_ => follower.Refresh()));
        }
    }
}