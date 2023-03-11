using System.ComponentModel.DataAnnotations;

namespace Pub.ResourceEngagement.Entities
{
    public class EngagementOrder
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal DailyRate { get; set; }

        public int EngagementDuration { get; set; }

        public decimal EngagementValue { get; set; }
    }
}
