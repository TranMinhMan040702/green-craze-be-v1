using green_craze_be_v1.Application.Model.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.Delivery
{
    public class DeliverySpecification : BaseSpecification<Domain.Entities.Delivery>
    {
        public DeliverySpecification(GetDeliveryPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.ToLower().Contains(keyword)
                || x.Price.ToString().Contains(keyword);
            }
            if (request.IsSortAccending)
            {
                if (request.ColumnName == nameof(Domain.Entities.Unit.Name))
                {
                    AddOrderBy(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Unit.Id))
                {
                    AddOrderBy(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Unit.UpdatedAt))
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
                if (request.ColumnName == nameof(Domain.Entities.Unit.Name))
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Unit.Id))
                {
                    AddOrderByDescending(x => x.Id);
                }
                else if (request.ColumnName == nameof(Domain.Entities.Unit.UpdatedAt))
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