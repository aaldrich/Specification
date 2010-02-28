using System;
using System.Linq.Expressions;

namespace Specification
{
    public class AnonymousSpecification<T> : Specification<T>
    {
        readonly Expression<Func<T, bool>> condition_expression;

        public AnonymousSpecification(Expression<Func<T,bool>> condition_expression)
        {
            this.condition_expression = condition_expression;
        }

        public override bool is_satisfied_by(T value)
        {
            return condition_expression.Compile().Invoke(value);
        }

        public override Expression<Func<T, bool>> is_satisfied_by()
        {
            return condition_expression;
        }
    }
}