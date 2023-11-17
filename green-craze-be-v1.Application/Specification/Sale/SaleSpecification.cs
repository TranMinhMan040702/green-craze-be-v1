using green_craze_be_v1.Application.Model.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Sale
{
    public class SaleSpecification : BaseSpecification<Domain.Entities.Sale>
    {
        public SaleSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Products);
        }

        public SaleSpecification(bool latest)
        {
            AddOrderByDescending(x => x.StartDate);
            ApplyPaging(1, 0);
            AddInclude(x => x.Products);
        }

        public SaleSpecification(GetSalePagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.Contains(keyword) || x.PromotionalPercent.ToString().Contains(keyword);
            }
            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAscending)
            {
                if (columnName == nameof(Domain.Entities.Sale.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Sale.StartDate).ToLower())
                {
                    AddOrderBy(x => x.StartDate);
                }
                else if (columnName == nameof(Domain.Entities.Sale.EndDate).ToLower())
                {
                    AddOrderBy(x => x.EndDate);
                }
                else if (columnName == nameof(Domain.Entities.Sale.PromotionalPercent).ToLower())
                {
                    AddOrderBy(x => x.PromotionalPercent);
                }
                else if (columnName == nameof(Domain.Entities.Sale.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Sale.UpdatedAt).ToLower())
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Sale.Status).ToLower())
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
                if (columnName == nameof(Domain.Entities.Sale.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Sale.StartDate).ToLower())
                {
                    AddOrderByDescending(x => x.StartDate);
                }
                else if (columnName == nameof(Domain.Entities.Sale.EndDate).ToLower())
                {
                    AddOrderByDescending(x => x.EndDate);
                }
                else if (columnName == nameof(Domain.Entities.Sale.PromotionalPercent).ToLower())
                {
                    AddOrderByDescending(x => x.PromotionalPercent);
                }
                else if (columnName == nameof(Domain.Entities.Sale.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Sale.UpdatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Sale.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            AddInclude(x => x.Products);
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}
