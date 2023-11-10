using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Order
{
    public class OrderItemSpecification : BaseSpecification<OrderItem>
    {
        public OrderItemSpecification(long orderId) : base(x => x.Order.Id == orderId)
        {
            AddInclude(x => x.Variant);
        }

        public OrderItemSpecification(long variantId, DateTime firstDate, DateTime lastDate, string status)
            : base(
                  x => x.Variant.Id == variantId
                  && x.Order.CreatedAt >= firstDate
                  && x.Order.CreatedAt <= lastDate
                  && x.Order.Status == status)
        { }
    }
}