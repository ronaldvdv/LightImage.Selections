using DynamicData;
using System;
using System.Linq;
using System.Reactive.Disposables;

namespace LightImage.Selections
{
    internal class SelectionSynchronizer<T> : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private bool _locked = false;

        public SelectionSynchronizer(ISelectionList<T> left, ISelectionList<T> right)
        {
            Bind(left, right).DisposeWith(_disposable);
            Bind(right, left).DisposeWith(_disposable);
        }

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
                        bool hasRefresh = cs.Any(c => c.Reason == ListChangeReason.Refresh);
                        _locked = true;
                        follower.Update(leader.Items.ToArray());
                        if (hasRefresh)
                        {
                            follower.Refresh();
                        }
                        _locked = false;
                    }
                }),
                leader.OnRefresh.Subscribe(_ => follower.Refresh())
            );
        }
    }
}