using System.Collections.Generic;
using Zoobook.Shared;

namespace Zoobook.Models
{
    public interface IRequestBaseDto
    {
        IEnumerable<long> Ids { get; set; }

        IPaging Paging { get; set; }
    }
}
