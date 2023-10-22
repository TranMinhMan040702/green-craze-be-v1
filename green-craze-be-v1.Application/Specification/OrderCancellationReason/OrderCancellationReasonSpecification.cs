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
                || x.Note.ToLower().Contains(keyword);
            }
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.OrderCancellationReason.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.OrderCancellationReason.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.OrderCancellationReason.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.OrderCancellationReason.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.OrderCancellationReason.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.OrderCancellationReason.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}