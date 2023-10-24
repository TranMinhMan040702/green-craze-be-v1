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
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Delivery.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Id).ToLower())
                {
                    AddOrderBy(x => x.Id);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Price).ToLower())
                {
                    AddOrderBy(x => x.Price);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.Delivery.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Id).ToLower())
                {
                    AddOrderByDescending(x => x.Id);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Price).ToLower())
                {
                    AddOrderByDescending(x => x.Price);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else if (columnName == nameof(Domain.Entities.Delivery.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
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