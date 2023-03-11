using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pub.ResourceEngagement.Dtos;
using Pub.ResourceEngagement.Entities;
using Pub.ResourceEngagement.Persistence;
using Pub.ResourceEngagement.Services;

namespace Pub.ResourceEngagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngagementOrderController : ControllerBase
    {
        private readonly IEngagementOrderService _engagementOrderService;

        public EngagementOrderController(IEngagementOrderService engagementOrderService)
        {
            _engagementOrderService = engagementOrderService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> PostEngagementOrder(EngagementOrderDto engagementOrderDto)
        {
            var engagementOrderId = await _engagementOrderService.Add(engagementOrderDto);
            return Ok(new { id = engagementOrderId });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutEngagementOrder(Guid engagementOrderId, EngagementOrderDto engagementOrderDto)
        {
            var result = await _engagementOrderService.Update(engagementOrderId, engagementOrderDto);
            if (result is null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetEngagementOrderBy(Guid engagementOrderId)
        {
            var result = await _engagementOrderService.GetById(engagementOrderId);

            if (result is null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _engagementOrderService.GetAll();

            return Ok(result);
        }
    }
}
