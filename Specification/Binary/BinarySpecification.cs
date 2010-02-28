using System.Linq.Expressions;

namespace Specification.Binary
{
    public abstract class BinarySpecification<T> : Specification<T>
    {
        protected ISpecification<T> left_side;
        protected ISpecification<T> right_side;

        protected BinarySpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left_side = left;
            this.right_side = right;
        }

        public abstract override bool is_satisfied_by(T value);
        public abstract override Expression<System.Func<T, bool>> is_satisfied_by();
    }
}