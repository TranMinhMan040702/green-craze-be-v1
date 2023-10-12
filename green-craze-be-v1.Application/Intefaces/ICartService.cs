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
        Task<long> AddVariantItemToCart(AddVariantItemToCartRequest request);

        Task<bool> UpdateCartItemQuantity(UpdateCartItemQuantityRequest request);

        Task<bool> DeleteCartItem(long cartItemId, string userId);

        Task<PaginatedResult<CartItemDto>> GetCartByUser(GetCartPagingRequest request);

        Task<CartItemDto> GetCartItemById(long cartItemId, string userId);
    }
}