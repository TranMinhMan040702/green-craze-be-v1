using green_craze_be_v1.Application.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Order
{
    public class OrderSpecification : BaseSpecification<Domain.Entities.Order>
    {
        public OrderSpecification(long orderId, string userId) : base(x => x.Id == orderId && x.User.Id == userId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(GetOrderPagingRequest request, bool isPaging = false)
        {
            AddInclude(x => x.User);
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(request.UserId))
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    Criteria = x => x.User.Id == request.UserId &&
                        x.Code.ToLower().Contains(keyword)
                        || x.Note.ToString().Contains(keyword);
                }
                else
                {
                    Criteria = x => x.User.Id == request.UserId;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    Criteria = x => x.Code.ToLower().Contains(keyword)
                        || x.Note.ToString().Contains(keyword);
                }
                else
                {
                    Criteria = x => true;
                }
            }
            if (request.IsSortAccending)
            {
                if (request.ColumnName == nameof(Domain.Entities.Order.Id))
                {
                    AddOrderBy(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Order.UpdatedAt))
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderBy(x => x.CreatedAt);
                }
            }
            else
            {
                if (request.ColumnName == nameof(Domain.Entities.Order.Id))
                {
                    AddOrderByDescending(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Order.UpdatedAt))
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
            }
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}