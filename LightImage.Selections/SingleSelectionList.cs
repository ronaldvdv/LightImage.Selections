using DynamicData;
using ReactiveUI;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace LightImage.Selections
{
    public class SingleSelectionList<T> : SelectionListAbstract<T>
    {
        private readonly Action<T> _setter;

        private SingleSelectionList(IObservable<IChangeSet<T>> getter, Action<T> setter) : base(getter)
        {
            _setter = setter;
        }

        public static SingleSelectionList<T> Create<TViewModel>(TViewModel vm, Expression<Func<TViewModel, T>> property)
        {
            var member = property.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"{nameof(property)} must be a {nameof(MemberExpression)}");
            }

            var name = member.Member.Name;
            var getter = vm.WhenAnyValue(property).ToZeroOneChangeSet();
            var setter = GetPropSetter<TViewModel, T>(name);

            return new SingleSelectionList<T>(getter, value => setter(vm, value));
        }

        public static Action<TObject, TProperty> GetPropSetter<TObject, TProperty>(string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TObject));

            ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);

            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            Action<TObject, TProperty> result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            return result;
        }

        public override void Update(params T[] items)
        {
            if (items != null && items.Length > 0)
                _setter(items.Last());
            else
                _setter(default);
        }
    }
}