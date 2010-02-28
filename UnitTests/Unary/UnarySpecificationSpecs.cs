using System;
using System.Linq.Expressions;
using Machine.Specifications;
using Moq;
using Specification;
using Specification.Unary;
using UnitTests.Stubs;
using It=Machine.Specifications.It;

namespace UnitTests.Unary
{
    public class not_specification_base
    {
        Establish c = () =>
            {
                specification = new Mock<ISpecification<Employee>>();
            };

        protected static NotSpecification<Employee> not_specification;
        protected static Expression<Func<Employee,bool>> result;
        protected static Employee employee;
        protected static Mock<ISpecification<Employee>> specification;
    }
    
    public class not_bool_specification_base : not_specification_base
    {
        protected static new bool result;
    }

    [Subject("Expression Not against a specification that is true")]
    public class when_using_expression_not_against_a_specification_that_satisfies_the_condition : not_specification_base
    {
        Establish c = () =>
        {
            specification.Setup(x => x.is_satisfied_by()).Returns(x=>true);

            not_specification = new NotSpecification<Employee>(specification.Object);
        };

        Because b = () =>
            result = not_specification.is_satisfied_by();

        It the_expression_should_evaluate_to_false = () =>
            result.Compile().Invoke(employee).ShouldBeFalse();
    }

    [Subject("Expression Not against a specification that is false")]
    public class when_using_expression_not_against_a_specification_that_does_not_satisfy_the_condition : not_specification_base
    {
        Establish c = () =>
        {
            specification.Setup(x => x.is_satisfied_by()).Returns(x => false);

            not_specification = new NotSpecification<Employee>(specification.Object);
        };

        Because b = () =>
            result = not_specification.is_satisfied_by();

        It the_expression_should_evaluate_to_true = () =>
            result.Compile().Invoke(employee).ShouldBeTrue();
    }

    [Subject("Bitwise Not against a specification that is true")]
    public class when_using_not_bitwise_against_a_specification_that_satisfies_the_condition : not_bool_specification_base
    {
        Establish c = () =>
            {
                specification.Setup(x => x.is_satisfied_by(employee)).Returns(true);

                not_specification = new NotSpecification<Employee>(specification.Object);
            };

        Because b = () =>
            result = not_specification.is_satisfied_by(employee);

        It should_return_false = () =>
            result.ShouldBeFalse();
    }

    [Subject("Bitwise Not against a specification that is false")]
    public class when_using_not_bitwise_against_a_specification_that_does_not_satisfies_the_condition : not_bool_specification_base
    {
        Establish c = () =>
        {
            specification.Setup(x => x.is_satisfied_by(employee)).Returns(false);

            not_specification = new NotSpecification<Employee>(specification.Object);
        };

        Because b = () =>
            result = not_specification.is_satisfied_by(employee);

        It should_return_true = () =>
            result.ShouldBeTrue();
    }
}