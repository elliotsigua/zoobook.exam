using Zoobook.Shared;

namespace Zoobook.Models
{
    public class EmployeeCreateDto
    {
        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Address { get; set; }

        [Email]
        [Required]
        public string Email { get; set; }

        public string MobilePhone { get; set; }
    }
}
