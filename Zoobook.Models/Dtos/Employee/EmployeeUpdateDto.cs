using Zoobook.Shared;

namespace Zoobook.Models
{
    public class EmployeeUpdateDto
    {
        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Address { get; set; }

        public string MobilePhone { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }
    }
}
