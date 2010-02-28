using System;
using System.Linq.Expressions;
using Machine.Specifications;
using Moq;
using Specification;
using Specification.Binary;
using UnitTests.Stubs;
using It=Machine.Specifications.It;

namespace UnitTests.Binary
{
    public abstract class andspecification_base
    {
        Establish c = () =>
            {
                left_specification = new Mock<ISpecification<Employee>>();
                right_specification = new Mock<ISpecification<Employee>>();
            };
        protected static Mock<ISpecification<Employee>> left_specification;
        protected static Mock<ISpecification<Employee>> right_specification;
        protected static AndSpecification<Employee> and_specification;
        protected static Expression<Func<Employee, bool>> result;
        protected static Employee employee;
    }

    public abstract class and_bool_specification_base : andspecification_base
    {
        protected static new bool result;
    }


    [Subject("Testing 2 Specifications that are both true using Expression.AndAlso")]
    public class when_asked_if_two_true_specifications_satisfy_the_given_condition : andspecification_base
    {
        Establish c = () =>
            {
                left_specification.Setup(x=>x.is_satisfied_by()).Returns(x=> true);
                right_specification.Setup(x=>x.is_satisfied_by()).Returns(x=>true);

                and_specification = new AndSpecification<Employee>(left_specification.Object, right_specification.Object);
            };

        Because b = () =>
            result = and_specification.is_satisfied_by();

        It the_expression_should_evaulate_to_true = () =>
            result.Compile().Invoke(employee).ShouldBeTrue();
    }

    [Subject("Testing 2 Specifications with 1 false using Expression.AndAlso")]
    public class when_asked_if_two_specifications_satisfy_a_condition_when_1_does_not : andspecification_base
    {
        Establish c = () =>
        {
            left_specification.Setup(x => x.is_satisfied_by()).Returns(x => false);
            right_specification.Setup(x => x.is_satisfied_by()).Returns(x => true);

            and_specification = new AndSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = and_specification.is_satisfied_by();

        It the_expression_should_evalutate_to_false = () =>
            result.Compile().Invoke(employee).ShouldBeFalse();
    }

    [Subject("Testing 2 Specifications with 1 false using bitwise and")]
    public class when_asked_if_two_boolean_specifications_satisfy_a_condition_when_1_does_not : and_bool_specification_base
    {
        Establish c = () =>
        {
            left_specification.Setup(x => x.is_satisfied_by(employee)).Returns(false);
            right_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

            and_specification = new AndSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = and_specification.is_satisfied_by(employee);

        It the_result_should_be_false = () =>
            result.ShouldBeFalse();
    }

    [Subject("Testing 2 Specifications that are true using bitwise and")]
    public class when_asked_if_two_true_boolean_specifications_satisfy_a_condition : and_bool_specification_base
    {
        Establish c = () =>
        {
            left_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);
            right_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

            and_specification = new AndSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = and_specification.is_satisfied_by(employee);

        It the_result_should_be_true= () =>
            result.ShouldBeTrue();
    }
}