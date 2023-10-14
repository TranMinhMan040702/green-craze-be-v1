﻿using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListSale([FromQuery] GetSalePagingRequest request)
        {
            var res = await _saleService.GetListSale(request);

            return Ok(APIResponse<PaginatedResult<SaleDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale([FromRoute] long id)
        {
            var res = await _saleService.GetSale(id);

            return Ok(APIResponse<SaleDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromForm] CreateSaleRequest request)
        {
            var saleId = await _saleService.CreateSale(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/sales/{saleId}";

            return Created(url, new APIResponse<object>(new { id = saleId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale([FromRoute] long id, [FromForm] UpdateSaleRequest request)
        {
            var res = await _saleService.UpdateSale(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale([FromRoute] long id)
        {
            var res = await _saleService.DeleteSale(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListSale([FromQuery] List<long> ids)
        {
            var res = await _saleService.DeleteListSale(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}
