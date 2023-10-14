using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.ProductImage;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;

        public ProductsController(IProductImageService productImageService, IProductService productService)
        {
            _productImageService = productImageService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListProduct([FromForm] GetProductPagingRequest request)
        {
            var res = await _productService.GetListProduct(request);

            return Ok(APIResponse<PaginatedResult<ProductDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            var res = await _productService.GetProduct(id);

            return Ok(APIResponse<ProductDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var productId = await _productService.CreateProduct(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/products/{productId}";

            return Created(url, new APIResponse<object>(new { id = productId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromForm] UpdateProductRequest request)
        {
            var res = await _productService.UpdateProduct(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var res = await _productService.DeleteProduct(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListProduct([FromQuery] List<long> ids)
        {
            var res = await _productService.DeleteListProduct(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetListProductImage([FromRoute] long productId)
        {
            var res = await _productImageService.GetListProductImage(productId);

            return Ok(APIResponse<List<ProductImageDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateProductImage
            ([FromForm] List<IFormFile> images, [FromRoute] long productId)
        {
            await _productImageService.CreateProductImage(images, productId);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/products/{productId}/images";

            return Created(url, new APIResponse<object>(new { id = productId }, StatusCodes.Status201Created));
        }

        [HttpPut("{productId}/images/{id}")]
        public async Task<IActionResult> UpdateProductImage([FromRoute] long id, [FromForm] IFormFile image)
        {
            var res = await _productImageService.UpdateProductImage(image, id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{productId}/images/{id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] long id)
        {
            var res = await _productImageService.DeleteProductImage(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{productId}/images")]
        public async Task<IActionResult> DeleteListProductImage([FromQuery] List<long> ids)
        {
            var res = await _productImageService.DeleteListProductImage(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}
