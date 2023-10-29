using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Inventory;
using green_craze_be_v1.Application.Specification.Docket;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Infrastructure.Repositories;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<DocketDto>> GetListDocketByProductId(long productId)
        {
            var dockets = await _unitOfWork.Repository<Docket>().ListAsync(new DocketSpecification(productId));
            List<DocketDto> docketDtos = new List<DocketDto>();
            dockets.ForEach(x => docketDtos.Add(_mapper.Map<DocketDto>(x)));
            return docketDtos;
        }

        public async Task<bool> ImportProduct(ImportProductRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                var product = await _unitOfWork.Repository<Product>()
                    .GetEntityWithSpec(new ProductSpecification(request.ProductId))
                    ?? throw new NotFoundException("Cannot find current product");
                product.Quantity += request.Quantity;
                product.ActualInventory = request.ActualInventory;
                var docket = new Docket
                {
                    Type = DOCKET_TYPE.IMPORT,
                    Code = StringUtil.GenerateUniqueCode(),
                    Quantity = request.Quantity,
                    Note = request.Note
                };
                product.Dockets.Add(docket);
                _unitOfWork.Repository<Product>().Update(product);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
                }

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
