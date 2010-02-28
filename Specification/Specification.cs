using System;
using System.Linq.Expressions;
using Specification.Binary;
using Specification.Unary;

namespace Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool is_satisfied_by(T value);
        public abstract Expression<Func<T, bool>> is_satisfied_by();

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}