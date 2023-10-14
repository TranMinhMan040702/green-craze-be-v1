using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Specification.Sale;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public SaleService (IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<PaginatedResult<SaleDto>> GetListSale(GetSalePagingRequest request)
        {
            var spec = new SaleSpecification(request, isPaging: true);
            var countSpec = new SaleSpecification(request);

            var sales = await _unitOfWork.Repository<Sale>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Sale>().CountAsync(countSpec);
            var saleDtos = new List<SaleDto>();
            sales.ForEach(x => saleDtos.Add(_mapper.Map<SaleDto>(x)));

            return new PaginatedResult<SaleDto>(saleDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<SaleDto> GetSale(long id)
        {
            var sale = await _unitOfWork.Repository<Sale>().GetById(id);

            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<long> CreateSale(CreateSaleRequest request)
        {
            var sale = _mapper.Map<Sale>(request);
            sale.Image = _uploadService.UploadFile(request.Image).Result;
            await _unitOfWork.Repository<Sale>().Insert(sale);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            return sale.Id;
        }

        public async Task<bool> UpdateSale(long id, UpdateSaleRequest request)
        {
            var sale = await _unitOfWork.Repository<Sale>().GetById(id);
            sale = _mapper.Map<UpdateSaleRequest, Sale>(request, sale);
            sale.Id = id;
            sale.Image = _uploadService.UploadFile(request.Image).Result;

            _unitOfWork.Repository<Sale>().Update(sale);
            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteSale(long id)
        {
            var sale = await _unitOfWork.Repository<Sale>().GetById(id);
            //sale.Status = false;
            _unitOfWork.Repository<Sale>().Update(sale);
            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListSale(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var sale = await _unitOfWork.Repository<Sale>().GetById(id);
                    //sale.Status = false;
                    _unitOfWork.Repository<Sale>().Update(sale);
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
