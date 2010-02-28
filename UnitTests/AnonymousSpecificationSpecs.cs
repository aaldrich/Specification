using System;
using System.Linq.Expressions;
using Machine.Specifications;
using Specification;
using UnitTests.Stubs;

namespace UnitTests
{
    public class anonymous_specification_base
    {
        protected static Expression<Func<Employee, bool>> expression;
        protected static AnonymousSpecification<Employee> anonymous_specification;
        protected static Expression<Func<Employee, bool>> result;
        protected static Employee employee;
    }

    public class anonymous_bool_specification_base : anonymous_specification_base
    {
        protected static new bool result;
    }

    [Subject("Evaluating Specification that matches the condition using Expression<Func<T,bool>>")]
    public class when_asked_the_specification_satisfies_the_given_condition_when_it_does : anonymous_specification_base
    {
        Establish c = () =>
            {
                employee = new Employee{Name = "Adam Aldrich"};

                Expression<Func<Employee, bool>> expression = x=>x.Name.Equals("Adam Aldrich");
                anonymous_specification = new AnonymousSpecification<Employee>(expression);
            };

        Because b = () =>
            result = anonymous_specification.is_satisfied_by();

        It the_expression_should_return_true = () =>
            result.Compile().Invoke(employee).ShouldBeTrue();
    }

    [Subject("Evaluating Specification that does not match the condition using Expression<Func<T,bool>>")]
    public class when_asked_the_specification_satisfies_the_given_condition_when_it_does_not : anonymous_specification_base
    {
        Establish c = () =>
        {
            employee = new Employee { Name = "robby" };

            Expression<Func<Employee, bool>> expression = x => x.Name.Equals("Adam Aldrich");
            anonymous_specification = new AnonymousSpecification<Employee>(expression);
        };

        Because b = () =>
            result = anonymous_specification.is_satisfied_by();

        It the_expression_should_return_true = () =>
            result.Compile().Invoke(employee).ShouldBeFalse();
    }

    [Subject("Evaluating Specification that matches the condition using bool")]
    public class when_asked_the_boolean_specification_satisfies_the_given_condition_when_it_does : anonymous_bool_specification_base
    {
        Establish c = () =>
        {
            employee = new Employee { Name = "Adam Aldrich" };

            Expression<Func<Employee, bool>> expression = x => x.Name.Equals("Adam Aldrich");
            anonymous_specification = new AnonymousSpecification<Employee>(expression);
        };

        Because b = () =>
            result = anonymous_specification.is_satisfied_by(employee);

        It should_return_true = () =>
            result.ShouldBeTrue();
    }

    [Subject("Evaluating Specification that does not match the condition using bool")]
    public class when_asked_the_boolean_specification_satisfies_the_given_condition_when_it_does_not : anonymous_bool_specification_base
    {
        Establish c = () =>
        {
            employee = new Employee { Name = "robby" };

            Expression<Func<Employee, bool>> expression = x => x.Name.Equals("Adam Aldrich");
            anonymous_specification = new AnonymousSpecification<Employee>(expression);
        };

        Because b = () =>
            result = anonymous_specification.is_satisfied_by(employee);

        It should_return_false = () =>
            result.ShouldBeFalse();
    }

}