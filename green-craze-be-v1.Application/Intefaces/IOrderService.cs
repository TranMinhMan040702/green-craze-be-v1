using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IOrderService
    {
        Task<long> CreateOrder(CreateOrderRequest request);

        Task<bool> UpdateOrder(UpdateOrderRequest request);

        Task<OrderDto> GetOrder(long id, string userId);

        Task<PaginatedResult<OrderDto>> GetListOrder(GetOrderPagingRequest request);

        Task<PaginatedResult<OrderDto>> GetListUserOrder(GetOrderPagingRequest request);
    }
}