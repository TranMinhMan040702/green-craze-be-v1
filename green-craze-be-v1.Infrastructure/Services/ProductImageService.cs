using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.ProductImage;
using green_craze_be_v1.Application.Specification.ProductImage;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public ProductImageService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<List<ProductImageDto>> GetListProductImage(long productId)
        {
            var productImages =
                await _unitOfWork.Repository<ProductImage>().ListAsync(new ProductImageSpecification(productId));
            var productImageDtos = new List<ProductImageDto>();
            productImages.ForEach(productImage => productImageDtos.Add(_mapper.Map<ProductImageDto>(productImage)));

            return productImageDtos;
        }

        public async Task CreateProductImage(List<IFormFile> images, long productId)
        {
            foreach (IFormFile image in images)
            {
                ProductImageDto productImageDto = new()
                {
                    ProductId = productId,
                    Image = _uploadService.UploadFile(image).Result,
                    Size = image.Length,
                    ContentType = image.ContentType
                };
                var productImage = _mapper.Map<ProductImage>(productImageDto);
                productImage.Product = await _unitOfWork.Repository<Product>().GetById(productId);
                await _unitOfWork.Repository<ProductImage>().Insert(productImage);
            }

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }
        }

        public async Task<bool> UpdateProductImage(IFormFile image, long id)
        {
            var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id);
            productImage.Image = _uploadService.UploadFile(image).Result;
            productImage.Size = image.Length;
            productImage.ContentType = image.ContentType;
            _unitOfWork.Repository<ProductImage>().Update(productImage);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteProductImage(long id)
        {
            var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id);
            _unitOfWork.Repository<ProductImage>().Delete(productImage);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListProductImage(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id);
                    _unitOfWork.Repository<ProductImage>().Delete(productImage);
                }
                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update entity");
                }
                await _unitOfWork.Commit();
                return isSuccess;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}