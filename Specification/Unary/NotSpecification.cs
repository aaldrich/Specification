using System;
using System.Linq.Expressions;

namespace Specification.Unary
{
    public class NotSpecification<T> : Specification<T>
    {
        readonly ISpecification<T> to_negate;

        public NotSpecification(ISpecification<T> to_negate)
        {
            this.to_negate = to_negate;
        }

        public override bool is_satisfied_by(T condition)
        {
            return !to_negate.is_satisfied_by(condition);
        }

        public override Expression<Func<T, bool>> is_satisfied_by()
        {
            var body = Expression.Not(to_negate.is_satisfied_by().Body);
            var lambda = Expression.Lambda<Func<T,bool>>(body, to_negate.is_satisfied_by().Parameters);

            return lambda;
        }
    }
}