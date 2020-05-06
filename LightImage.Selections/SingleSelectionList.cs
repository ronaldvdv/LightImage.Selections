using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list bound to a view model property holding zero or one selected items.
    /// </summary>
    /// <typeparam name="T">Type of selected items.</typeparam>
    public class SingleSelectionList<T> : SelectionListAbstract<T>
    {
        private readonly Action<T> _setter;

        private SingleSelectionList(IObservable<T> getter, Action<T> setter)
            : base(getter.ToZeroOneChangeSet())
        {
            _setter = setter;
        }

        /// <summary>
        /// Construct a <see cref="SingleSelectionList{T}"/> based on a given view model and property expression.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="vm">The view model.</param>
        /// <param name="property">Property of the view model.</param>
        /// <returns>The selection list.</returns>
        public static SingleSelectionList<T> Create<TViewModel>(TViewModel vm, Expression<Func<TViewModel, T>> property)
        {
            if (!(property.Body is MemberExpression member))
            {
                throw new ArgumentException($"{nameof(property)} must be a {nameof(MemberExpression)}");
            }

            var name = member.Member.Name;
            var getter = vm.WhenAnyValue(property);
            var setter = GetPropSetter<TViewModel, T>(name);

            return new SingleSelectionList<T>(getter, value => setter(vm, value));
        }

        /// <inheritdoc/>
        public override void Update(params T[] items)
        {
            if (items != null && items.Length > 0)
            {
                _setter(items.Last());
            }
            else
            {
                _setter(default);
            }
        }

        private static Action<TObject, TProperty> GetPropSetter<TObject, TProperty>(string propertyName)
        {
            var objectParameter = Expression.Parameter(typeof(TObject));
            var valueParameter = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyExpression = Expression.Property(objectParameter, propertyName);
            var assignment = Expression.Assign(propertyExpression, valueParameter);
            var result = Expression.Lambda<Action<TObject, TProperty>>(assignment, objectParameter, valueParameter).Compile();
            return result;
        }
    }
}