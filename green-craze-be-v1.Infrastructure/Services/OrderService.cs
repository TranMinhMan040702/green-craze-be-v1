using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Cart;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.User;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IDateTimeService dateTimeService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        private async Task<Order> InitOrder(AppUser user, PaymentMethod paymentMethod,
            Address userAddress, Delivery delivery, CreateOrderRequest request)
        {
            //var paymentStatus = paymentMethod.Code == PAYMENT_CODE.PAYPAL;

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetById(item.VariantId)
                    ?? throw new InvalidRequestException("Unexpected variantId");
                var variantPrice = item.Quantity * variant.TotalPrice;
                totalAmount += variantPrice;
                orderItems.Add(new OrderItem()
                {
                    Quantity = item.Quantity,
                    Variant = variant,
                    UnitPrice = variant.TotalPrice,
                    TotalPrice = variantPrice
                });
            }

            var tax = totalAmount * ORDER_TAX.TAX;
            totalAmount = totalAmount + delivery.Price + tax;

            var transaction = new Transaction()
            {
                PaymentMethod = paymentMethod.Code,
                //PaidAt = paymentStatus ? _dateTimeService.Current : null,
                TotalPay = totalAmount,
                //PaypalOrderId = request.PaypalOrderId,
                //PaypalOrderStatus = request.PaypalOrderStatus
            };

            var order = new Order()
            {
                User = user,
                Address = userAddress,
                DeliveryMethod = delivery.Name,
                Transaction = transaction,
                OrderItems = orderItems,
                Note = request.Note,
                ShippingCost = delivery.Price,
                Tax = 0.1,
                Code = StringUtil.GenerateUniqueCode(),
                Status = ORDER_STATUS.NOT_PROCESSED,
                PaymentStatus = false,
                TotalAmount = totalAmount
            };

            return order;
        }

        private async Task UpdateProductQuantity(CreateOrderRequest request, Order order)
        {
            foreach (var item in request.Items)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(item.VariantId))
                    ?? throw new InvalidRequestException("Unexpected variantId");
                var product = variant.Product ?? throw new InvalidRequestException("Unexpected variantId");

                var q = item.Quantity * variant.Quantity;

                var docket = new Docket()
                {
                    Code = StringUtil.GenerateUniqueCode(),
                    Product = product,
                    Order = order,
                    Type = DOCKET_TYPE.EXPORT,
                    Quantity = q,
                };

                await _unitOfWork.Repository<Docket>().Insert(docket);

                product.Quantity -= q;
                product.Sold = q;

                _unitOfWork.Repository<Product>().Update(product);
            }
        }

        private async Task UpdateUserCart(CreateOrderRequest request, Cart cart)
        {
            foreach (var item in request.Items)
            {
                var cartItem = await _unitOfWork.Repository<CartItem>()
                    .GetEntityWithSpec(new CartItemSpecification(cart.Id, item.VariantId))
                    ?? throw new InvalidRequestException("Unexpected variantId");

                _unitOfWork.Repository<CartItem>().Delete(cartItem);
            }
        }

        public async Task<string> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(request.UserId))
                    ?? throw new NotFoundException("Cannot find current user");
                var userAddress = user.Addresses.FirstOrDefault(x => x.IsDefault == true)
                    ?? throw new NotFoundException("Cannot find default user's address");

                var delivery = await _unitOfWork.Repository<Delivery>().GetById(request.DeliveryId)
                    ?? throw new InvalidRequestException("Unexpected deliveryId");

                var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(request.PaymentMethodId)
                    ?? throw new InvalidRequestException("Unexpected paymentMethodId");

                var order = await InitOrder(user, paymentMethod, userAddress, delivery, request);
                await _unitOfWork.Repository<Order>().Insert(order);

                await UpdateProductQuantity(request, order);

                await UpdateUserCart(request, user.Cart);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to create order, an error has occured");
                }
                await _unitOfWork.Commit();

                return order.Code;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<PaginatedResult<OrderDto>> GetListOrder(GetOrderPagingRequest request)
        {
            var orders = await _unitOfWork.Repository<Order>().ListAsync(new OrderSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<Order>().CountAsync(new OrderSpecification(request));

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));
                var dto = _mapper.Map<OrderDto>(order);
                dto.Items = await GetOrderItemDto(listOrderItem);
                orderDtos.Add(dto);
            }

            return new PaginatedResult<OrderDto>(orderDtos, request.PageIndex, count, request.PageSize);
        }

        public Task<PaginatedResult<OrderDto>> GetListUserOrder(GetOrderPagingRequest request)
        {
            return GetListOrder(request);
        }

        private async Task<List<OrderItemDto>> GetOrderItemDto(List<OrderItem> listOrderItem)
        {
            var listItems = new List<OrderItemDto>();
            foreach (var oi in listOrderItem)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(oi.Variant.Id))
                ?? throw new NotFoundException("Cannot find varaint item");

                var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(variant.Product.Id))
                    ?? throw new NotFoundException("Cannot find product of variant item");

                var orderItemDto = _mapper.Map<OrderItemDto>(oi);

                orderItemDto.VariantQuantity = variant.Quantity;
                orderItemDto.VariantName = variant.Name;
                orderItemDto.Sku = product.Code + "-" + variant.Sku;
                orderItemDto.ProductName = product.Name;
                orderItemDto.ProductSlug = product.Slug;
                orderItemDto.ProductUnit = product.Unit.Name;
                orderItemDto.ProductImage = product.Images.FirstOrDefault(x => x.IsDefault)?.Image ?? product.Images.FirstOrDefault()?.Image;

                listItems.Add(orderItemDto);
            }
            return listItems;
        }

        public async Task<OrderDto> GetOrder(long id)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(id))
                ?? throw new InvalidRequestException("Unexpected orderId");
            var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));

            var listItems = await GetOrderItemDto(listOrderItem);

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.Items = listItems;

            return orderDto;
        }

        public async Task<bool> UpdateOrder(UpdateOrderRequest request)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(request.OrderId, request.UserId))
                ?? throw new InvalidRequestException("Unexpected orderId");

            // Cannot update order while it's delivered
            if (order.Status == ORDER_STATUS.DELIVERED)
                throw new InvalidRequestException("Unexpected order status, cannot update order while it's delivered");

            if (order.Status == ORDER_STATUS.CANCELLED
                && (!_currentUserService.IsInRole(USER_ROLE.ADMIN) && !_currentUserService.IsInRole(USER_ROLE.STAFF)))
                throw new InvalidRequestException("Unexpected order status, user cannot update order while it's cancelled");

            // Customer cannot cancel order while it's processing, only admin and staff can do that
            if (order.Status != ORDER_STATUS.NOT_PROCESSED
                && request.Status == ORDER_STATUS.CANCELLED
                && (!_currentUserService.IsInRole(USER_ROLE.ADMIN) && !_currentUserService.IsInRole(USER_ROLE.STAFF)))
                throw new InvalidRequestException("Unexpected order status, cannot cancel order while it's processing");

            var now = _dateTimeService.Current;
            order.Status = request.Status;
            if (request.Status == ORDER_STATUS.DELIVERED)
            {
                order.PaymentStatus = true;
                if (order.Transaction.PaymentMethod == PAYMENT_CODE.COD)
                    order.Transaction.PaidAt = now;
                order.Transaction.CompletedAt = now;
            }
            else if (request.Status == ORDER_STATUS.CANCELLED)
            {
                if (string.IsNullOrEmpty(request.OtherCancellation))
                {
                    var cancellationReason = await _unitOfWork.Repository<OrderCancellationReason>()
                        .GetById(request.OrderCancellationReasonId)
                        ?? throw new InvalidRequestException("Unexpected orderCancellationReasonId");
                    order.CancelReason = cancellationReason;
                }
                else
                {
                    order.OtherCancelReason = request.OtherCancellation;
                }
            }
            else
            {
                order.CancelReason = null;
                order.OtherCancelReason = null;
            }

            _unitOfWork.Repository<Order>().Update(order);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess)
            {
                throw new Exception("Cannot handle to update order, an error has occured");
            }

            return true;
        }

        public async Task<OrderDto> GetOrderByCode(string code, string userId)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(code, userId))
                ?? throw new InvalidRequestException("Unexpected order code");
            var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));

            var listItems = await GetOrderItemDto(listOrderItem);

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.Items = listItems;

            return orderDto;
        }

        public async Task<bool> CompletePaypalOrder(CompletePaypalOrderRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(request.OrderId, request.UserId))
                ?? throw new InvalidRequestException("Unexpected order id");

                order.Transaction.PaidAt = _dateTimeService.Current;
                order.Transaction.PaypalOrderId = request.PaypalOrderId;
                order.Transaction.PaypalOrderStatus = request.PaypalOrderStatus;
                order.PaymentStatus = true;
                order.Status = ORDER_STATUS.PROCESSING;

                _unitOfWork.Repository<Order>().Update(order);

                await _unitOfWork.Save();
                await _unitOfWork.Commit();

                return true;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}