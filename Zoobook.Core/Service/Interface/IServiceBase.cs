using System.Net.Http;

namespace Zoobook.Core
{
    public interface IServiceBase
    {
        HttpClient ApiHttpClient();
    }
}
