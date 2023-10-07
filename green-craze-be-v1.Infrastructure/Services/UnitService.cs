using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Specification.Unit;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<bool> CreateUnit(CreateUnitRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var unit = _mapper.Map<Unit>(request);
                await _unitOfWork.Repository<Unit>().Insert(unit);

                var isSuccess = await _unitOfWork.Save() > 0;

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<bool> DeleteMany(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                foreach (var id in ids)
                {
                    var unit = await _unitOfWork.Repository<Unit>().GetById(id);
                    unit.Status = false;
                    _unitOfWork.Repository<Unit>().Update(unit);
                }

                var isSuccess = await _unitOfWork.Save() > 0;

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<bool> DeleteUnit(long id)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var unit = await _unitOfWork.Repository<Unit>().GetById(id);

                unit.Status = false;
                _unitOfWork.Repository<Unit>().Update(unit);
                var isSuccess = await _unitOfWork.Save() > 0;

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<PaginatedResult<UnitDto>> GetAllUnit(GetUnitPagingRequest request)
        {
            var spec = new UnitSpecification(request, isPaging: true);
            var countSpec = new UnitSpecification(request);

            var units = await _unitOfWork.Repository<Unit>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Unit>().CountAsync(countSpec);
            var unitDtos = new List<UnitDto>();
            units.ForEach(x => unitDtos.Add(_mapper.Map<UnitDto>(x)));
            return new PaginatedResult<UnitDto>(unitDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<UnitDto> GetUnit(long id)
        {
            var unit = await _unitOfWork.Repository<Unit>().GetById(id);

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<bool> UpdateUnit(UpdateUnitRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var unit = await _unitOfWork.Repository<Unit>().GetById(request.Id);
                unit.Name = request.Name;
                unit.Status = request.Status;
                _unitOfWork.Repository<Unit>().Update(unit);
                var isSuccess = await _unitOfWork.Save() > 0;

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}