using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;

        public CartsController(ICartService cartService, ICurrentUserService currentUserService)
        {
            _cartService = cartService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVariantItemToCart([FromBody] AddVariantItemToCartRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var id = await _cartService.AddVariantItemToCart(request);
            if (id == -1)
                return Ok(new APIResponse<bool>(true, StatusCodes.Status204NoContent));
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/carts/{id}";
            return Created(url, new APIResponse<object>(new { id }, StatusCodes.Status201Created));
        }

        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromRoute] long cartItemId, [FromBody] UpdateCartItemQuantityRequest request)
        {
            request.CartItemId = cartItemId;
            request.UserId = _currentUserService.UserId;
            var resp = await _cartService.UpdateCartItemQuantity(request);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] long cartItemId)
        {
            var resp = await _cartService.DeleteCartItem(cartItemId, _currentUserService.UserId);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpGet]
        public async Task<IActionResult> GetCartByUser([FromQuery] GetCartPagingRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var resp = await _cartService.GetCartByUser(request);

            return Ok(new APIResponse<PaginatedResult<CartItemDto>>(resp, StatusCodes.Status200OK));
        }

        [HttpGet("{cartItemId}")]
        public async Task<IActionResult> GetCartByUser([FromRoute] long cartItemId)
        {
            var resp = await _cartService.GetCartItemById(cartItemId, _currentUserService.UserId);

            return Ok(new APIResponse<CartItemDto>(resp, StatusCodes.Status200OK));
        }
    }
}