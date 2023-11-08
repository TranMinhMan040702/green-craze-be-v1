using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Specification.Order
{
    public class OrderItemSpecification : BaseSpecification<OrderItem>
    {
        public OrderItemSpecification(long orderId) : base(x => x.Order.Id == orderId)
        {
            AddInclude(x => x.Variant);
        }

        public OrderItemSpecification(long orderItemId, string status) : base(x => x.Id == orderItemId && x.Order.Status == status)
        {
        }
    }
}