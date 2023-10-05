using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUnitService
    {
        Task<bool> CreateUnit(CreateUnitRequest request);

        Task<bool> UpdateUnit(UpdateUnitRequest request);

        Task<bool> DeleteUnit(long id);

        Task<bool> DeleteMany(List<long> ids);

        Task<PaginatedResult<UnitDto>> GetAllUnit(GetUnitPagingRequest request);

        Task<UnitDto> GetUnit(long id);
    }
}