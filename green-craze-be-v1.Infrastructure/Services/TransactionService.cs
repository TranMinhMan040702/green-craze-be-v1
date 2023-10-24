using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Transaction;
using green_craze_be_v1.Application.Specification.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<TransactionDto>> GetListTransaction(GetTransactionPagingRequest request)
        {
            var transactions = await _unitOfWork.Repository<Domain.Entities.Transaction>()
                .ListAsync(new TransactionSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<Domain.Entities.Transaction>()
                .CountAsync(new TransactionSpecification(request));

            return new PaginatedResult<TransactionDto>(transactions.Select(x =>
            {
                var res = _mapper.Map<TransactionDto>(x);
                res.OrderCode = x.Order.Code;
                return res;
            }).ToList(),
                request.PageIndex, count, request.PageSize);
        }

        public async Task<TransactionDto> GetTransaction(long id)
        {
            var transaction = await _unitOfWork.Repository<Domain.Entities.Transaction>()
                .GetEntityWithSpec(new TransactionSpecification(id));

            var dto = _mapper.Map<TransactionDto>(transaction);
            dto.OrderCode = transaction.Order.Code;

            return dto;
        }
    }
}