using Moq;
using MockQueryable.Moq;
using Pub.ResourceEngagement.Controllers;
using Pub.ResourceEngagement.Dtos;
using Pub.ResourceEngagement.Entities;
using Pub.ResourceEngagement.Persistence;
using Pub.ResourceEngagement.Services;
using Xunit;

namespace Pub.ResourceEngagement.UnitTest
{
    public class ResourceEngagementUnitTest
    {
        private readonly Mock<IEngagementOrderService> _engagementOrderService;
        private readonly EngagementOrderController _engagementOrderController;
        private readonly List<EngagementOrder> _engagementOrders;

        public ResourceEngagementUnitTest()
        {
            var dbContext = new Mock<IApplicationDbContext>();

            _engagementOrders = new List<EngagementOrder>();
            
            var mockSet = _engagementOrders
                .AsQueryable()
                .BuildMockDbSet();

            dbContext.Setup(_ => _.EngagementOrder).Returns(mockSet.Object);

            _engagementOrderService = new Mock<IEngagementOrderService>();

            _engagementOrderController = new EngagementOrderController(_engagementOrderService.Object);
        }

        [Fact]
        public async Task CreateEnagementOrder_WhenCreate()
        {
            //arrange
            var engagementOrderDto = new EngagementOrderDto
            {
                FirstName = "Mizanur",
                LastName = "Rahman",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(5),
                DailyRate = 100,
                EngagementDuration = 50
            };

            //act
            var result = await _engagementOrderController.PostEngagementOrder(engagementOrderDto);

            //assert
            _engagementOrderService.Verify(x => x.Add(
                It.Is<EngagementOrderDto>(eo =>
                eo.FirstName == engagementOrderDto.FirstName
                && eo.LastName == engagementOrderDto.LastName
                && eo.StartDate == engagementOrderDto.StartDate
                && eo.EndDate == engagementOrderDto.EndDate
                && eo.DailyRate == engagementOrderDto.DailyRate
                && eo.EngagementDuration == engagementOrderDto.EngagementDuration
                )), Times.Once);
        }
    }
}