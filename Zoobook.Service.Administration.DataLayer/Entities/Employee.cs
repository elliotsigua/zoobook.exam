using Zoobook.Core;
using Zoobook.Shared;

namespace Zoobook.Service.Administration.DataLayer
{
    public class Employee : EntityBase
    {
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string FirstName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public EmployeeStatus Status { get; set; }
    }
}
