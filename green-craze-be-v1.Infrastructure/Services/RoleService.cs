using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Role;
using green_craze_be_v1.Application.Specification.Role;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RoleDto>> GetListRole(GetRolePagingRequest request)
        {
            var roles = await _unitOfWork.Repository<AppRole>().ListAsync(new RoleSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<AppRole>().CountAsync(new RoleSpecification(request));

            return new PaginatedResult<RoleDto>(roles.Select(x => _mapper.Map<RoleDto>(x)).ToList(), request.PageIndex, count, request.PageSize);
        }

        public async Task<RoleDto> GetRole(string roleId)
        {
            var role = await _unitOfWork.Repository<AppRole>().GetById(roleId);

            return _mapper.Map<RoleDto>(role);
        }
    }
}