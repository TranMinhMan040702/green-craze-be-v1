using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class StaffsController : ControllerBase
    {
        private readonly IUserService _userService;

        public StaffsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromForm] CreateStaffRequest request)
        {
            var userId = await _userService.CreateStaff(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/units/{userId}";

            return Created(url, new APIResponse<object>(new { id = userId }, StatusCodes.Status201Created));
        }

        [HttpPut("{staffId}")]
        public async Task<IActionResult> UpdateStaff([FromRoute] long staffId, [FromForm] UpdateStaffRequest request)
        {
            request.Id = staffId;
            var res = await _userService.UpdateStaff(request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaff([FromRoute] long staffId)
        {
            var res = await _userService.GetStaff(staffId);

            return Ok(new APIResponse<StaffDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet]
        public async Task<IActionResult> GetListStaff([FromQuery] GetUserPagingRequest request)
        {
            var res = await _userService.GetListStaff(request);

            return Ok(new APIResponse<PaginatedResult<StaffDto>>(res, StatusCodes.Status200OK));
        }
    }
}