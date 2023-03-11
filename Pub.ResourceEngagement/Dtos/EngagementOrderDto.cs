namespace Pub.ResourceEngagement.Dtos
{
    public class EngagementOrderDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal DailyRate { get; set; }

        public int EngagementDuration { get; set; }
    }
}
