using System.Collections.Generic;

namespace Zoobook.Models
{
    public class EmployeeRequestDto : RequestBaseDto
    {
        public IEnumerable<string> Emails { get; set; }
    }
}
