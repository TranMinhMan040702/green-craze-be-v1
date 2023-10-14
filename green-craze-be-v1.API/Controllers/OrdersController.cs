using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentService;

        public OrdersController(IOrderService orderService, ICurrentUserService currentService)
        {
            _orderService = orderService;
            _currentService = currentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var id = await _orderService.CreateOrder(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/orders/{id}";

            return Created(url, new APIResponse<object>(new { id }, StatusCodes.Status201Created));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetListOrder([FromQuery] GetOrderPagingRequest request)
        {
            request.UserId = null;
            var orders = await _orderService.GetListOrder(request);

            return Ok(new APIResponse<PaginatedResult<OrderDto>>(orders, StatusCodes.Status200OK));
        }

        [HttpGet("me/list")]
        public async Task<IActionResult> GetListUserOrder([FromQuery] GetOrderPagingRequest request)
        {
            request.UserId = _currentService.UserId;
            var orders = await _orderService.GetListUserOrder(request);

            return Ok(new APIResponse<PaginatedResult<OrderDto>>(orders, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] long id)
        {
            var order = await _orderService.GetOrder(id, _currentService.UserId);

            return Ok(new APIResponse<OrderDto>(order, StatusCodes.Status200OK));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] long id, [FromBody] UpdateOrderRequest request)
        {
            request.OrderId = id;
            request.UserId = _currentService.UserId;

            var isSuccess = await _orderService.UpdateOrder(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }
    }
}