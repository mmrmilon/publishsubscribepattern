namespace Pub.ResourceEngagement.Options
{
    public class PublisherOptions
    {
        public string HostName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int PortNumber { get; set; } = 15672;

        public string? ExchangeName { get; set; }

        public string? ExchangeType { get; set; }

        public List<string> QueueList { get; set; } = new List<string>();
    }
}
