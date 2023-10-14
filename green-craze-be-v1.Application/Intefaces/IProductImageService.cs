using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.ProductImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetListProductImage(long productId);
        Task CreateProductImage(List<IFormFile> images, long productId);
        Task<bool> UpdateProductImage(IFormFile image, long id);
        Task<bool> DeleteListProductImage(List<long> ids);
        Task<bool> DeleteProductImage(long id);
    }
}
