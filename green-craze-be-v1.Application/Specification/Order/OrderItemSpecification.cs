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
            AddInclude(x => x.Order);
            AddInclude(x => x.Variant);
        }
    }
}