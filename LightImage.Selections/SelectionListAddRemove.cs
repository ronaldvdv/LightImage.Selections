using DynamicData;
using System;
using System.Linq;

namespace LightImage.Selections
{
    public abstract class SelectionListAddRemove<T> : SelectionListAbstract<T>
    {
        public SelectionListAddRemove(IObservable<IChangeSet<T>> changes) : base(changes)
        {
        }

        public override void Update(params T[] items)
        {
            var remove = Items.Except(items).ToArray();
            foreach (var item in remove)
                Remove(item);

            var add = items.Except(Items).ToArray();
            foreach (var item in add)
                Add(item);
        }

        protected abstract void Add(T item);

        protected abstract void Remove(T item);
    }
}