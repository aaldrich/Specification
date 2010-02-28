using System;
using System.Linq.Expressions;

namespace Specification
{
    public interface ISpecification<T>
    {
        bool is_satisfied_by(T condition);
        Expression<Func<T, bool>> is_satisfied_by(); 
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> Not();
    }
}