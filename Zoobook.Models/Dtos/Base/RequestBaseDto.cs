using System.Collections.Generic;
using Zoobook.Shared;

namespace Zoobook.Models
{
    public class RequestBaseDto : IRequestBaseDto
    {
        public IEnumerable<long> Ids { get; set; }

        public IPaging Paging { get; set; }
    }
}
