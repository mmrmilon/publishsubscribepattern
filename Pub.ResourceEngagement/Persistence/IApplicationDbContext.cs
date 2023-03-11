using Microsoft.EntityFrameworkCore;
using Pub.ResourceEngagement.Entities;
using System.Collections.Generic;

namespace Pub.ResourceEngagement.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<EngagementOrder> EngagementOrder { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
