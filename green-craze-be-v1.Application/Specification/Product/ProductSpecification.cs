using green_craze_be_v1.Application.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Product
{
    public class ProductSpecification : BaseSpecification<Domain.Entities.Product>
    {
        public ProductSpecification()
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }
        public ProductSpecification(string slug) : base(x => x.Slug == slug)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }
        public ProductSpecification(GetProductPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name == keyword;
            }
            if (query.IsSortAccending)
            {
                if (query.ColumnName == nameof(Domain.Entities.Product.Name))
                {
                    AddOrderBy(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Product.CreatedAt))
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Product.UpdatedAt))
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (query.ColumnName == nameof(Domain.Entities.Product.Name))
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Product.CreatedAt))
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Product.UpdatedAt))
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}