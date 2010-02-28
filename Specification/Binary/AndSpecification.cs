using System;
using System.Linq.Expressions;

namespace Specification.Binary
{
    public class AndSpecification<T> : BinarySpecification<T> 
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right) : base(left,right){}

        public override bool is_satisfied_by(T condition)
        {
            return left_side.is_satisfied_by(condition) && right_side.is_satisfied_by(condition);
        }

        public override Expression<Func<T, bool>> is_satisfied_by()
        {
            var body = Expression.AndAlso(left_side.is_satisfied_by().Body, right_side.is_satisfied_by().Body);
            var lambda = Expression.Lambda<Func<T,bool>>(body, left_side.is_satisfied_by().Parameters);

            return lambda;
        }
    }
}