using Zoobook.Shared;

namespace Zoobook.Models
{
    public class EmployeeDto : EntityBaseDto
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
