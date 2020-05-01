using DynamicData;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace LightImage.Selections
{
    public interface ISelectionList<T> : IObservableList<T>, IEnumerable<T>
    {
        IObservable<Unit> OnRefresh { get; }

        void Refresh();

        void Update(params T[] item);
    }
}