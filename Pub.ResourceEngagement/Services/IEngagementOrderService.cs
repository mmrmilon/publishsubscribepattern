using Pub.ResourceEngagement.Dtos;

namespace Pub.ResourceEngagement.Services
{
    public interface IEngagementOrderService
    {
        Task<Guid> Add(EngagementOrderDto engagementOrderDto);
        Task<EngagementOrderDto?> Update(Guid engagementOrderId, EngagementOrderDto engagementOrderDto);
        Task<List<EngagementOrderDto>> GetAll();
        Task<EngagementOrderDto?> GetById(Guid engagementOrderId);
    }
}
