using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.UserFollowProduct;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/user-follow-products")]
    [ApiController]
    public class UserFollowProductController : ControllerBase
    {
        private readonly IUserFollowProductService _userFollowProductService;

        public UserFollowProductController(IUserFollowProductService userFollowProductService)
        {
            _userFollowProductService = userFollowProductService;
        }

        [HttpPost("like")]
        public async Task<ActionResult> UserLikeProduct([FromBody] FollowProductRequest request)
        {
            await _userFollowProductService.LikeProduct(request);

            return Created("", new APIResponse<object>(new { id = request.ProductId }, StatusCodes.Status201Created));
        }
    }
}
