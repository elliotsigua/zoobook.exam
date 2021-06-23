using NodaTime;

namespace Zoobook.Shared
{
    public class InstantRange
    {
        public Instant? MinDate { get; set; }

        public Instant? MaxDate { get; set; }
    }
}
