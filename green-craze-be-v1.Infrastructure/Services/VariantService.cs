using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Variant;
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

        public async Task<long> CreateVariant(CreateVariantRequest request)
        {
            var variant = _mapper.Map<Variant>(request);
            await _unitOfWork.Repository<Variant>().Insert(variant);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            // NOTE: xem lai id cua variant
            return 0;
        }

        Task<bool> IVariantService.DeleteListVariant(List<long> ids)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVariantService.DeleteVariant(long id)
        {
            throw new NotImplementedException();
        }

        Task<PaginatedResult<VariantDto>> IVariantService.GetListVariant(GetVariantPagingRequest request)
        {
            throw new NotImplementedException();
        }

        Task<VariantDto> IVariantService.GetVariant(long id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IVariantService.UpdateVariant(long id, UpdateVariantRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
