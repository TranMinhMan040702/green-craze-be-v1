using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IAddressService
    {
        public Task<long> CreateAddress(CreateAddressRequest request);

        public Task<bool> UpdateAddress(UpdateAddressRequest request);

        public Task<bool> DeleteAddress(long id, string userId);

        public Task<AddressDto> GetAddress(long id, string userId);

        public Task<PaginatedResult<AddressDto>> GetListAddress(GetAddressPagingRequest request);

        public Task<bool> SetAddressDefault(long id, string userId);

        Task<List<ProvinceDto>> GetListProvince();

        Task<List<DistrictDto>> GetListDistrictByProvince(long provinceId);

        Task<List<WardDto>> GetListWardByDistrict(long districtId);
    }
}