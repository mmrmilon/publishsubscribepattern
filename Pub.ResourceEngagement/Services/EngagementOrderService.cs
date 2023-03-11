using Microsoft.EntityFrameworkCore;
using Pub.ResourceEngagement.Dtos;
using Pub.ResourceEngagement.Entities;
using Pub.ResourceEngagement.Persistence;

namespace Pub.ResourceEngagement.Services
{
    public class EngagementOrderService : IEngagementOrderService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPublisherService _publisherService;
        public EngagementOrderService(IApplicationDbContext dbContext, IPublisherService publisherService)
        {
            _dbContext = dbContext;
            _publisherService = publisherService;
        }

        public async Task<Guid> Add(EngagementOrderDto engagementOrderDto)
        {
            var engagementOrder = new EngagementOrder
            {
                Id = Guid.NewGuid(),
                FirstName = engagementOrderDto.FirstName,
                LastName = engagementOrderDto.LastName,
                StartDate = engagementOrderDto.StartDate,
                EndDate = engagementOrderDto.EndDate,
                DailyRate = engagementOrderDto.DailyRate,
                EngagementDuration = engagementOrderDto.EngagementDuration,
                EngagementValue = engagementOrderDto.DailyRate * engagementOrderDto.EngagementDuration
            };

            _dbContext.EngagementOrder.Add(engagementOrder);
            await _dbContext.SaveChangesAsync(default);

            //publish message
            _publisherService.PublishMessage(engagementOrder, string.Empty, string.Empty, "EngagementOrders");

            return engagementOrder.Id;
        }

        public async Task<EngagementOrderDto?> Update(Guid engagementOrderId, EngagementOrderDto engagementOrderDto)
        {
            var engagementOrder = await _dbContext.EngagementOrder.Where(x => x.Id == engagementOrderId).FirstOrDefaultAsync();
            if(engagementOrder is not null) 
            {
                engagementOrder.FirstName= engagementOrderDto.FirstName;
                engagementOrder.LastName = engagementOrderDto.LastName;
                engagementOrder.StartDate = engagementOrderDto.StartDate;
                engagementOrder.EndDate= engagementOrderDto.EndDate;
                engagementOrder.DailyRate= engagementOrderDto.DailyRate;
                engagementOrder.EngagementDuration= engagementOrderDto.EngagementDuration;
                engagementOrder.EngagementValue = engagementOrderDto.DailyRate * engagementOrderDto.EngagementDuration;

                _dbContext.EngagementOrder.Update(engagementOrder);
                await _dbContext.SaveChangesAsync(default);
            }

            var result = await GetById(engagementOrderId);

            //publish message
            if (result is not null)
            {
                _publisherService.PublishMessage(engagementOrder, "", "", "");
            }

            return result;
        }

        public async Task<List<EngagementOrderDto>> GetAll()
        {
            return await _dbContext.EngagementOrder
                .Select(x => new EngagementOrderDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DailyRate = x.DailyRate,
                    EngagementDuration = x.EngagementDuration
                }).ToListAsync();
        }

        public async Task<EngagementOrderDto?> GetById(Guid engagementOrderId)
        {
            var result = await _dbContext.EngagementOrder
                .Where(x => x.Id == engagementOrderId)
                .Select(s => new EngagementOrderDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    DailyRate = s.DailyRate,
                    EngagementDuration = s.EngagementDuration
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
