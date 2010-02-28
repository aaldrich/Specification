using System.Data;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Machine.Specifications;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Linq;
using Specification;
using Specification.Unary;

namespace IntegrationTests.Employees
{
    public class employee_base
    {
        Establish c = () =>
            {
                session_factory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory)
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Employee>())
                    .ExposeConfiguration(x=> config = x)
                    .BuildSessionFactory();

                connection = session_factory.OpenSession().Connection;

                create_database(config);

                put_data_in_database();

                session = session_factory.OpenSession(connection);

                initialize_nhibernate_profiler(); //If you have NHibernate Profiler
            };

        static void initialize_nhibernate_profiler()
        {
            HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();
        }

        static void create_database(Configuration configuration)
        {
            new SchemaExport(configuration).Execute(false,true,false,connection,null);
        }

        static void put_data_in_database()
        {
            session = session_factory.OpenSession(connection);

            adam_aldrich = new Employee { Age = 27, Id = 0, Name = "Adam Aldrich" };
            caleb_aldrich = new Employee { Age = 4, Id = 0, Name = "Caleb Aldrich" };
            chelsea_aldrich = new Employee { Age = 2, Id = 0, Name = "Chelsea Aldrich" };
            adam_aldrich_diff_age = new Employee(){Age = 28,Id = 0,Name = "Adam Aldrich"};

            session.Save(adam_aldrich);
            session.Save(caleb_aldrich);
            session.Save(chelsea_aldrich);
            session.Save(adam_aldrich_diff_age);
            session.Flush();
            session.Close();
        }

        protected static ISessionFactory session_factory;
        protected static ISession session;
        static IDbConnection connection;
        static Configuration config;
        protected static Employee adam_aldrich;
        protected static Employee caleb_aldrich;
        protected static Employee chelsea_aldrich;
        protected static Employee adam_aldrich_diff_age;
    }

    [Subject("Anonymous Specification for all Employees with a given name")]
    public class when_querying_for_all_employees_with_a_given_name_using_anonymous_specification : employee_base
    {
        Establish c = () =>
            {
                specification = 
                    new AnonymousSpecification<Employee>(x => x.Name.Equals("Adam Aldrich"));
            };

        Because b = () =>
            result = session.Linq<Employee>().Where(specification.is_satisfied_by());

        It should_return_all_the_employees_with_the_matching_name = () =>
            result.ShouldContainOnly(adam_aldrich,adam_aldrich_diff_age);

        static AnonymousSpecification<Employee> specification;
        static IQueryable<Employee> result;
    }

    [Subject("Joining 2 Anonymous Specifications with OrSpecfication")]
    public class when_querying_using_or_specification : employee_base
    {
        Establish c = () =>
            {
                specification = 
                    new AnonymousSpecification<Employee>(x => x.Name.Equals("Adam Aldrich"))
                .Or(new AnonymousSpecification<Employee>(x => x.Age.Equals(4)));
            };

        Because b = () =>
            results = session.Linq<Employee>().Where(specification.is_satisfied_by());

        It should_return_only_employees_that_match_one_or_both_or_conditions = () =>
            results.ShouldContainOnly(adam_aldrich,caleb_aldrich,adam_aldrich_diff_age);

        static ISpecification<Employee> specification;
        static IQueryable<Employee> results;
    }

    [Subject("Joining 2 Anonymous Specifications with AndSpecfication")]
    public class when_querying_using_and_specification : employee_base
    {
        Establish c = () =>
        {
            specification =
                new AnonymousSpecification<Employee>(x => x.Name.Equals("Adam Aldrich"))
            .And(new AnonymousSpecification<Employee>(x => x.Age.Equals(27)));
        };

        Because b = () =>
            results = session.Linq<Employee>().Where(specification.is_satisfied_by());

        It should_return_only_employees_that_match_both_or_conditions = () =>
            results.ShouldContainOnly(adam_aldrich);

        static ISpecification<Employee> specification;
        static IQueryable<Employee> results;
    }

    [Subject("Querying using Not Specfication")]
    public class when_querying_using_not_specification : employee_base
    {
        Establish c = () =>
        {
            specification = 
                new NotSpecification<Employee>(
                    new AnonymousSpecification<Employee>(x => x.Name.Equals("Adam Aldrich")));
        };

        Because b = () =>
            results = session.Linq<Employee>().Where(specification.is_satisfied_by());

        It should_return_only_employees_that_dont_match_the_given_conditions = () =>
            results.ShouldContainOnly(caleb_aldrich,chelsea_aldrich);

        static ISpecification<Employee> specification;
        static IQueryable<Employee> results;
    }
}