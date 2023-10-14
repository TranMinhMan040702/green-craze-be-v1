using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Cart;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddVariantItemToCart(AddVariantItemToCartRequest request)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of current user");

            var cartItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpec(new CartItemSpecification(cart.Id, request.VariantId));

            var ci = new CartItem();
            if (cartItem == null)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetById(request.VariantId)
                    ?? throw new InvalidRequestException("Unexpected variantId");
                ci.Quantity = request.Quantity;
                ci.Variant = variant;
                cart.CartItems.Add(ci);
                _unitOfWork.Repository<Cart>().Update(cart);
            }
            else
            {
                cartItem.Quantity += 1;
                _unitOfWork.Repository<CartItem>().Update(cartItem);
            }

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to add product to your cart, an error has occured");

            return cartItem == null ? ci.Id : -1;
        }

        public async Task<bool> DeleteCartItem(long cartItemId, string userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(userId))
                ?? throw new NotFoundException("Cannot find cart of current user");
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == cartItemId)
                ?? throw new InvalidRequestException("Unexpected cartItemId");

            cart.CartItems.Remove(cartItem);
            _unitOfWork.Repository<Cart>().Update(cart);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to remove product from your cart, an error has occured");

            return true;
        }

        public async Task<PaginatedResult<CartItemDto>> GetCartByUser(GetCartPagingRequest request)
        {
            var cartItems = await _unitOfWork.Repository<CartItem>().ListAsync(new CartItemSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<CartItem>().CountAsync(new CartItemSpecification(request));

            var cartDtos = new List<CartItemDto>();
            cartItems.ForEach(x =>
            {
                cartDtos.Add(GetCartItemById(x));
            });

            return new PaginatedResult<CartItemDto>(cartDtos, request.PageIndex, count, request.PageSize);
        }

        private CartItemDto GetCartItemById(CartItem cartItem)
        {
            var isPromotion = cartItem.Variant.PromotionalItemPrice != cartItem.Variant.ItemPrice;

            return new CartItemDto()
            {
                Id = cartItem.Id,
                CreatedAt = cartItem.CreatedAt,
                CreatedBy = cartItem.CreatedBy,
                UpdatedAt = cartItem.UpdatedAt,
                UpdatedBy = cartItem.UpdatedBy,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Variant.Quantity * cartItem.Variant.ItemPrice,
                TotalPromotionalPrice = isPromotion ? cartItem.Variant.Quantity * cartItem.Variant.PromotionalItemPrice.Value : null,
                Sku = cartItem.Variant.Sku,
                VariantName = cartItem.Variant.Name,
                VariantPrice = cartItem.Variant.ItemPrice,
                VariantPromotionalPrice = isPromotion ? cartItem.Variant.PromotionalItemPrice.Value : null,
                IsPromotion = isPromotion,
                VariantQuantity = cartItem.Variant.Quantity,
            };
        }

        public async Task<bool> UpdateCartItemQuantity(UpdateCartItemQuantityRequest request)
        {
            if (request.Quantity <= 0)
                throw new InvalidRequestException("Unexpected quantity, it must be a positive number");
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of current user");

            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == request.CartItemId)
                ?? throw new InvalidRequestException("Unexpected cartItemId");

            cartItem.Quantity = request.Quantity;

            _unitOfWork.Repository<Cart>().Update(cart);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to update product quantity, an error has occured");

            return true;
        }
    }
}