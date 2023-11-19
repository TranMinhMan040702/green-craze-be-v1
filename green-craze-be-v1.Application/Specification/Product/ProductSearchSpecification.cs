using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Product
{
    public class ProductSearchSpecification : BaseSpecification<Domain.Entities.Product>
    {
        public ProductSearchSpecification(SearchProductPagingRequest query, bool isPaging = false)
        {
            Criteria = x => x.Name.Contains(query.Search);

            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAscending)
            {
                AddOrderBy(x => x.Name);
            }
            else
            {
                AddOrderByDescending(x => x.Name);
            }
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}