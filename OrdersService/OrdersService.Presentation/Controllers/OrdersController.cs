using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdersService.Application.Dtos;
using OrdersService.Application.Interfaces;
using OrdersService.Presentation.Hubs;
using System.Security.Claims;

namespace OrdersService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrdersController(
            IOrderService orderService,
            IHubContext<OrderHub> hubContext)
        {
            _orderService = orderService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<ActionResult<OrderNotificationDto>> CreateOrderAsync(OrderCreateDto orderCreateDto)
        {
            var clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orderNotificationDto = await _orderService.CreateOrderAsync(orderCreateDto, clientId);

            await _hubContext.Clients.Group("drivers").SendAsync("GetDriverNotification", orderNotificationDto);

            return Ok(orderNotificationDto);
        }

        [Authorize(Roles = "Client, Driver")]
        [HttpPut("decline/{id}")]
        public async Task<ActionResult> DeclineOrderAsync(string orderId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            await _orderService.DeclineOrderAsync(orderId, role);

            return Ok(orderId);
        }

        [Authorize(Roles = "Driver")]
        [HttpPut("accept")]
        public async Task<ActionResult> AcceptOrderAsync([FromQuery] string orderId)
        {
            var driverId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _orderService.AcceptOrderAsync(orderId, driverId);

            return Ok(orderId);

        }

        [Authorize(Roles = "Driver")]
        [HttpPut("complete")]
        public async Task<ActionResult> CompleteOrderAsync([FromQuery]string orderId)
        {
            await _orderService.CompleteOrderAsync(orderId);
            
            return Ok(orderId);
        }

        [Authorize(Roles = "Client,Driver")]
        [HttpPut("feedback")]
        public async Task<ActionResult> AddMarkToOrderAsync([FromQuery]string orderId, int mark)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            await _orderService.SetMarkToOrderAsync(orderId, mark, role);

            return Ok(orderId);
        }
    }
}
