using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Statistic;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("total")]
        public async Task<IActionResult> StatisticTotal()
        {
            var res = await _statisticService.StatisticTotal();

            return Ok(APIResponse<StatisticTotalResponse>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> StatisticRevenue([FromQuery] StatisticRevenueRequest request)
        {
            var res = await _statisticService.StatisticRevenue(request);

            return Ok(APIResponse<List<StatisticRevenueResponse>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("top-selling-product-year")]
        public async Task<IActionResult> StatisticTopSellingProductYear([FromQuery] StatisticTopSellingProductYearRequest request)
        {
            var res = await _statisticService.StatisticTopSellingProductYear(request);

            return Ok(APIResponse<List<StatisticTopSellingProductYearResponse>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("order-status")]
        public async Task<IActionResult> StatisticOrderStatus([FromQuery] StatisticOrderStatusRequest request)
        {
            var res = await _statisticService.StatisticOrderStatus(request);

            return Ok(APIResponse<List<StatisticOrderStatusResponse>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("rating")]
        public async Task<IActionResult> StatisticRating([FromQuery] StatisticRatingRequest request)
        {
            var res = await _statisticService.StatisticRating(request);

            return Ok(APIResponse<List<StatisticRatingResponse>>.Initialize(res, StatusCodes.Status200OK));
        }
    }
}
