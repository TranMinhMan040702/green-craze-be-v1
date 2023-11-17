using green_craze_be_v1.Application.Model.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.ProductCategory
{
    public class ProductCategorySpecification : BaseSpecification<Domain.Entities.ProductCategory>
    {
        public ProductCategorySpecification(string slug) : base(x => x.Slug == slug) { }
        public ProductCategorySpecification(GetProductCategoryPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                if (query.Status)
                    Criteria = x => (x.Name.Contains(keyword) || x.Slug.Contains(keyword)) && x.Status == true;
                else
                    Criteria = x => x.Name.Contains(keyword) || x.Slug.Contains(keyword);
            }
            else
            {
                if (query.Status)
                    Criteria = x => x.Status == true;
                else
                    Criteria = x => true;
            }
            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAscending)
            {
                if (columnName == nameof(Domain.Entities.ProductCategory.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.Slug).ToLower())
                {
                    AddOrderBy(x => x.Slug);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.UpdatedAt).ToLower())
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.Status).ToLower())
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
                if (columnName == nameof(Domain.Entities.ProductCategory.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.Slug).ToLower())
                {
                    AddOrderByDescending(x => x.Slug);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.UpdatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.ProductCategory.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}
