using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Variant;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class VariantService : IVariantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VariantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VariantDto>> GetListVariant(GetVariantPagingRequest request)
        {
            var spec = new VariantSpecification(request, isPaging: true);
            var countSpec = new VariantSpecification(request);
            var variants = await _unitOfWork.Repository<Variant>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Variant>().CountAsync(countSpec);
            var variantDtos = new List<VariantDto>();
            variants.ForEach(variant =>
            {
                var variantDto = _mapper.Map<VariantDto>(variant);
                variantDtos.Add(variantDto);
            });

            return new PaginatedResult<VariantDto>(variantDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<VariantDto> GetVariant(long id)
        {
            var spec = new VariantSpecification();
            var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(spec);
            var variantDto = _mapper.Map<VariantDto>(variant);

            return variantDto;
        }

        public async Task<long> CreateVariant(CreateVariantRequest request)
        {
            var variant = _mapper.Map<Variant>(request);
            variant.Product = await _unitOfWork.Repository<Product>().GetById(request.ProductId);
            variant.Status = VARIANT_STATUS.ACTIVE;
            await _unitOfWork.Repository<Variant>().Insert(variant);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            return variant.Id;
        }

        public async Task<bool> UpdateVariant(long id, UpdateVariantRequest request)
        {
            var variant = await _unitOfWork.Repository<Variant>().GetById(id);
            variant = _mapper.Map<UpdateVariantRequest, Variant>(request, variant);
            variant.Product = await _unitOfWork.Repository<Product>().GetById(request.ProductId);
            variant.Id = id;
            variant.Status = variant.Status switch
            {
                VARIANT_STATUS.ACTIVE => VARIANT_STATUS.ACTIVE,
                VARIANT_STATUS.INACTIVE => VARIANT_STATUS.INACTIVE,
                _ => throw new InvalidRequestException("Unexpected variant status: " + request.Status),
            };
            _unitOfWork.Repository<Variant>().Update(variant);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteVariant(long id)
        {
            var variant = await _unitOfWork.Repository<Variant>().GetById(id);
            variant.Status = VARIANT_STATUS.INACTIVE;
            _unitOfWork.Repository<Variant>().Update(variant);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListVariant(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var variant = await _unitOfWork.Repository<Variant>().GetById(id);
                    variant.Status = VARIANT_STATUS.INACTIVE;
                    _unitOfWork.Repository<Variant>().Update(variant);
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
