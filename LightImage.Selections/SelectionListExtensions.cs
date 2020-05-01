using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace LightImage.Selections
{
    public static class SelectionListExtensions
    {
        public static IDisposable BindPrimaryItem<TOwner, TItem>(this ISelectionList<TItem> list, TOwner owner, Expression<Func<TOwner, TItem>> property) where TOwner : class
        {
            return list.Connect().Select(_ => list.LastOrDefault()).BindTo(owner, property);
        }

        public static ISelectionList<T> GetSelectionList<T>(this ReadOnlyObservableCollection<T> source) where T : ISelectable
        {
            return new SelectableItemList<T>(source);
        }

        public static ISelectionList<T> GetSelectionList<TViewModel, T>(this TViewModel vm, Expression<Func<TViewModel, T>> member)
        {
            return SingleSelectionList<T>.Create(vm, member);
        }

        public static IDisposable Synchronize<T>(this ISelectionList<T> left, ISelectionList<T> right)
        {
            return new SelectionSynchronizer<T>(left, right);
        }
    }
}