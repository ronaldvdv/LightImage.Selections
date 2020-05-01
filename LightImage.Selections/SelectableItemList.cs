using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace LightImage.Selections
{
    public class SelectableItemList<T> : SelectionListAddRemove<T> where T : ISelectable
    {
        public SelectableItemList(ReadOnlyObservableCollection<T> source)
            : base(source.ToObservableChangeSet().AutoRefreshOnObservable(a => a.WhenAnyValue(x => x.IsSelected)).Filter(a => a.IsSelected))
        {
        }

        protected override void Add(T item)
        {
            item.IsSelected = true;
        }

        protected override void Remove(T item)
        {
            item.IsSelected = false;
        }
    }
}