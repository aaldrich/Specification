using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;

namespace IntegrationTests.Employees
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Age);
            Map(x => x.Name);
        }
    }
}