using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListReview([FromQuery] GetReviewPagingRequest request)
        {
            var res = await _reviewService.GetListReview(request);

            return Ok(APIResponse<PaginatedResult<ReviewDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview([FromRoute] long id)
        {
            var res = await _reviewService.GetReview(id);

            return Ok(new APIResponse<ReviewDto>(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] CreateReviewRequest request)
        {
            var reviewId = await _reviewService.CreateReview(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/reviews/{reviewId}";
            return Created(url, new APIResponse<object>(new { id = reviewId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReplyReview([FromBody] ReplyReviewRequest request, [FromRoute] long id)
        {
            var res = await _reviewService.ReplyReview(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] long id)
        {
            var res = await _reviewService.DeleteReview(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListReview([FromQuery] List<long> ids)
        {
            var res = await _reviewService.DeleteListReview(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}
