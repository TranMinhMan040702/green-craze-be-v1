using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.ProductCategory;
using green_craze_be_v1.Application.Specification.Brand;
using green_craze_be_v1.Application.Specification.ProductCategory;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<long> CreateProductCategory(CreateProductCategoryRequest request)
        {
            var productCategory = _mapper.Map<ProductCategory>(request);
            productCategory.Image = _uploadService.UploadFile(request.Image).Result;
            await _unitOfWork.Repository<ProductCategory>().Insert(productCategory);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            return productCategory.Id;
        }

        public async Task<bool> DeleteListProductCategory(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var productCategory = await _unitOfWork.Repository<ProductCategory>().GetById(id);
                    productCategory.Status = false;
                    _unitOfWork.Repository<ProductCategory>().Update(productCategory);
                }
                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
                }

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<bool> DeleteProductCategory(long id)
        {
            var productCategory = await _unitOfWork.Repository<ProductCategory>().GetById(id);
            productCategory.Status = false;
            _unitOfWork.Repository<ProductCategory>().Update(productCategory);
            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<PaginatedResult<ProductCategoryDto>> GetListProductCategory(GetProductCategoryPagingRequest request)
        {
            var spec = new ProductCategorySpecification(request, isPaging: true);
            var countSpec = new ProductCategorySpecification(request);

            var productCategories = await _unitOfWork.Repository<ProductCategory>().ListAsync(spec);
            var count = await _unitOfWork.Repository<ProductCategory>().CountAsync(countSpec);
            var productCategoryDtos = new List<ProductCategoryDto>();
            productCategories.ForEach(x => productCategoryDtos.Add(_mapper.Map<ProductCategoryDto>(x)));

            return new PaginatedResult<ProductCategoryDto>(productCategoryDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<ProductCategoryDto> GetProductCategory(long id)
        {
            var productCategory = await _unitOfWork.Repository<ProductCategory>().GetById(id);

            return _mapper.Map<ProductCategoryDto>(productCategory);
        }

        public async Task<bool> UpdateProductCategory(long id, UpdateProductCategoryRequest request)
        {
            var productCategory = await _unitOfWork.Repository<ProductCategory>().GetById(id);
            productCategory = _mapper.Map<UpdateProductCategoryRequest, ProductCategory>(request, productCategory);
            productCategory.Id = id;
            productCategory.Image = _uploadService.UploadFile(request.Image).Result;

            _unitOfWork.Repository<ProductCategory>().Update(productCategory);
            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }
    }
}
