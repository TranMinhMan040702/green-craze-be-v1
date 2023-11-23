using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface ICartService
    {
        Task<bool> AddVariantItemToCart(CreateCartItemRequest request);

        Task<bool> UpdateCartItemQuantity(UpdateCartItemRequest request);

        Task<bool> DeleteCartItem(long cartItemId, string userId);

        Task<bool> DeleteListCartItem(List<long> ids, string userId);

        Task<PaginatedResult<CartItemDto>> GetCartByUser(GetCartPagingRequest request);

        Task<List<CartItemDto>> GetCartItemByIds(List<long> ids);
    }
}