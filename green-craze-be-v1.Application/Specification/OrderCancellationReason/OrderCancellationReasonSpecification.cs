using green_craze_be_v1.Application.Model.OrderCancellationReason;
using green_craze_be_v1.Application.Model.PaymentMethod;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.OrderCancellationReason
{
    public class OrderCancellationReasonSpecification : BaseSpecification<Domain.Entities.OrderCancellationReason>
    {
        public OrderCancellationReasonSpecification(GetOrderCancellationReasonPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.ToLower().Contains(keyword)
                || x.Note.ToString().Contains(keyword);
            }
            if (request.IsSortAccending)
            {
                if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.Name))
                {
                    AddOrderBy(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.Id))
                {
                    AddOrderBy(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.UpdatedAt))
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
                if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.Name))
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.Id))
                {
                    AddOrderByDescending(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.OrderCancellationReason.UpdatedAt))
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