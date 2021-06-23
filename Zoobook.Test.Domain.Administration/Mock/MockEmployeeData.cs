using Zoobook.Core;
using Zoobook.Service.Administration.DataLayer;
using Zoobook.Shared;

namespace Zoobook.Test.Domain.Administration
{
    public class MockEmployeeData : IMockEntityData
    {
        public static MockEmployeeData Instance()
            => new MockEmployeeData();

        public void CreateMockData(ZoobookContextBase context)
        {
            context.Set<Employee>().AddRange(
                new Employee()
                {
                    Email = "robb.stark@house-stark.got",
                    FirstName = "Rob",
                    MiddleName = "Young Wolf",
                    LastName = "Stark",
                    MobilePhone = string.Empty,
                    Address = "Stark House",
                    Status = EmployeeStatus.Inactive
                },
                new Employee()
                {
                    Email = "tyrion.lannister@house-lannister.got",
                    FirstName = "Tyrion",
                    MiddleName = "The Imp",
                    LastName = "Lannister",
                    MobilePhone = string.Empty,
                    Address = "Lannister House",
                    Status = EmployeeStatus.Active
                },
                new Employee()
                {
                    Email = "daenerys.targaryens@house-targaryens.got",
                    FirstName = "Daenerys",
                    MiddleName = "Mother of Dragons",
                    LastName = "Targaryens",
                    MobilePhone = string.Empty,
                    Address = "Targaryens House",
                    Status = EmployeeStatus.Inactive
                },
                new Employee()
                {
                    Email = "olenna.tyrell@house-tyrell.got",
                    FirstName = "Olenna",
                    MiddleName = "Queen of Thorns",
                    LastName = "Tyrell",
                    MobilePhone = string.Empty,
                    Address = "Tyrell House",
                    Status = EmployeeStatus.Inactive
                });
            context.SaveChanges();
        }
    }
}
