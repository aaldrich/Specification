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
    public abstract class orspecification_base
    {
        protected static Mock<ISpecification<Employee>> left_specification;
        protected static Mock<ISpecification<Employee>> right_specification;
        protected static OrSpecification<Employee> or_specification;
        protected static Expression<Func<Employee, bool>> result;
        protected static Employee employee;
    }

    public abstract class or_bool_specification_base : orspecification_base
    {
        protected static new bool result;
    }


    [Subject("Testing 2 Specifications that are both true using Expression.OrElse")]
    public class when_oring_specification_with_two_true_specifications_satisfy_the_given_condition : orspecification_base
    {
        Establish c = () =>
            {
                left_specification = new Mock<ISpecification<Employee>>();
                left_specification.Setup(x=>x.is_satisfied_by()).Returns(x=> true);
                
                right_specification = new Mock<ISpecification<Employee>>();
                right_specification.Setup(x=>x.is_satisfied_by()).Returns(x=>true);

                or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
            };

        Because b = () =>
            result = or_specification.is_satisfied_by();

        It the_expression_should_evaulate_to_true = () =>
            result.Compile().Invoke(employee).ShouldBeTrue();
    }

    [Subject("Testing 2 Specifications with 1 false using Expression.OrElse")]
    public class when_oring_specification_with_1_false_specification : orspecification_base
    {
        Establish c = () =>
        {
            left_specification = new Mock<ISpecification<Employee>>();
            left_specification.Setup(x => x.is_satisfied_by()).Returns(x => false);

            right_specification = new Mock<ISpecification<Employee>>();
            right_specification.Setup(x => x.is_satisfied_by()).Returns(x => true);

            or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = or_specification.is_satisfied_by();

        It the_expression_should_evalutate_to_true = () =>
            result.Compile().Invoke(employee).ShouldBeTrue();
    }

    [Subject("Testing 2 Specifications with 1 false using Expression.OrElse")]
    public class when_oring_specification_with_2_false_specifications : orspecification_base
    {
        Establish c = () =>
        {
            left_specification = new Mock<ISpecification<Employee>>();
            left_specification.Setup(x => x.is_satisfied_by()).Returns(x => false);

            right_specification = new Mock<ISpecification<Employee>>();
            right_specification.Setup(x => x.is_satisfied_by()).Returns(x => false);

            or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = or_specification.is_satisfied_by();

        It the_expression_should_evalutate_to_false = () =>
            result.Compile().Invoke(employee).ShouldBeFalse();
    }

    [Subject("Testing 2 Specifications with 1 false using bitwise or")]
    public class when_oring_two_boolean_specifications_where_1_satisfies_the_condition_when_1_does_not : or_bool_specification_base
    {
        Establish c = () =>
        {
            left_specification = new Mock<ISpecification<Employee>>();
            left_specification.Setup(x => x.is_satisfied_by(employee)).Returns(false);

            right_specification = new Mock<ISpecification<Employee>>();
            right_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

            or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = or_specification.is_satisfied_by(employee);

        It the_result_should_be_true = () =>
            result.ShouldBeTrue();
    }

    [Subject("Testing 2 Specifications that are true using bitwise or")]
    public class when_oring_two_true_boolean_specifications_satisfy_a_condition : or_bool_specification_base
    {
        Establish c = () =>
        {
            left_specification = new Mock<ISpecification<Employee>>();
            left_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

            right_specification = new Mock<ISpecification<Employee>>();
            right_specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

            or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = or_specification.is_satisfied_by(employee);

        It the_result_should_be_true= () =>
            result.ShouldBeTrue();
    }

    [Subject("Testing 2 false Specifications using bitwise or")]
    public class when_oring_two_boolean_specifications_that_dont_match_the_condition : or_bool_specification_base
    {
        Establish c = () =>
        {
            left_specification = new Mock<ISpecification<Employee>>();
            left_specification.Setup(x => x.is_satisfied_by(employee)).Returns(false);

            right_specification = new Mock<ISpecification<Employee>>();
            right_specification.Setup(x => x.is_satisfied_by(employee)).Returns(false);

            or_specification = new OrSpecification<Employee>(left_specification.Object, right_specification.Object);
        };

        Because b = () =>
            result = or_specification.is_satisfied_by(employee);

        It the_result_should_be_false = () =>
            result.ShouldBeFalse();
    }

}