using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.ProductCategory;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Unit;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductImageService _productImageService;
        private readonly IUploadService _uploadService;

        public ProductService(
            IUnitOfWork unitOfWork, IMapper mapper, IProductImageService productImageService, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productImageService = productImageService;
            _uploadService = uploadService;
        }

        public async Task<PaginatedResult<ProductDto>> GetListProduct(GetProductPagingRequest request)
        {
            var spec = new ProductSpecification(request, isPaging: true);
            var countSpec = new ProductSpecification(request);
            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var productDtos = new List<ProductDto>();
            products.ForEach(product =>
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDtos.Add(productDto);
            });

            return new PaginatedResult<ProductDto>(productDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<ProductDto> GetProduct(long id)
        {
            var spec = new ProductSpecification();
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<long> CreateProduct(CreateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.Category = await _unitOfWork.Repository<ProductCategory>().GetById(request.CategoryId);
            product.Brand = await _unitOfWork.Repository<Brand>().GetById(request.BrandId);
            product.Unit = await _unitOfWork.Repository<Unit>().GetById(request.UnitId);
            if (request.SaleId != null)
            {
                product.Sale = await _unitOfWork.Repository<Sale>().GetById(request.SaleId);
            }
            product.Status = PRODUCT_STATUS.ACTIVE;
            
            List<ProductImage> productImages = new();
            foreach (IFormFile image in request.ProductImages)
            {
                ProductImage productImage = new()
                {
                    Image = _uploadService.UploadFile(image).Result,
                    Size = image.Length,
                    ContentType = image.ContentType
                };
                productImages.Add(productImage);
            }
            product.Images = productImages;
            await _unitOfWork.Repository<Product>().Insert(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }
            return product.Id;
        }

        public async Task<bool> UpdateProduct(long id, UpdateProductRequest request)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id);
            product = _mapper.Map<UpdateProductRequest, Product>(request, product);
            product.Id = id;
            product.Category = await _unitOfWork.Repository<ProductCategory>().GetById(request.CategoryId);
            product.Brand = await _unitOfWork.Repository<Brand>().GetById(request.BrandId);
            product.Unit = await _unitOfWork.Repository<Unit>().GetById(request.UnitId);
            if (request.SaleId != null)
            {
                product.Sale = await _unitOfWork.Repository<Sale>().GetById(request.SaleId);
            }
            product.Status = product.Status switch
            {
                PRODUCT_STATUS.ACTIVE => PRODUCT_STATUS.ACTIVE,
                PRODUCT_STATUS.INACTIVE => PRODUCT_STATUS.INACTIVE,
                PRODUCT_STATUS.SOLD_OUT => PRODUCT_STATUS.SOLD_OUT,
                _ => throw new InvalidRequestException("Unexpected product status: " + request.Status),
            };
            _unitOfWork.Repository<Product>().Update(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id);
            product.Status = PRODUCT_STATUS.INACTIVE;
            _unitOfWork.Repository<Product>().Update(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListProduct(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var product = await _unitOfWork.Repository<Product>().GetById(id);
                    product.Status = PRODUCT_STATUS.INACTIVE;
                    _unitOfWork.Repository<Product>().Update(product);
                }
                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
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
