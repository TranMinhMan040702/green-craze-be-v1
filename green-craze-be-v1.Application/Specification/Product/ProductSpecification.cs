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

        public ProductSpecification(long categoryId, bool category = true) : base(x => x.Category.Id == categoryId)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(long saleId, bool category = true, bool sale = true) 
            : base(x => x.Sale.Id == saleId)
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
                if (!string.IsNullOrEmpty(query.CategorySlug))
                {
                    Criteria = x => x.Name.ToLower().Contains(keyword) && x.Category.Slug == query.CategorySlug;
                } else
                {
                    Criteria = x => x.Name.ToLower().Contains(keyword);
                }
            } else
            {
                if (!string.IsNullOrEmpty(query.CategorySlug))
                {
                    Criteria = x => x.Category.Slug == query.CategorySlug;
                } else
                {
                    Criteria = x => true;
                }
            }
            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Product.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Product.Sold).ToLower())
                {
                    AddOrderBy(x => x.Sold);
                }
                else if (columnName == nameof(Domain.Entities.Product.Quantity).ToLower())
                {
                    AddOrderBy(x => x.Quantity);
                }
                else if (columnName == nameof(Domain.Entities.Product.ActualInventory).ToLower())
                {
                    AddOrderBy(x => x.ActualInventory);
                }
                else if (columnName == nameof(Domain.Entities.Product.Code).ToLower())
                {
                    AddOrderBy(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.Product.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else if (columnName == nameof(Domain.Entities.Product.Category).ToLower())
                {
                    AddOrderBy(x => x.Category.Name);
                }
                else if (columnName == nameof(Domain.Entities.Product.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Product.UpdatedAt).ToLower())
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
                if (columnName == nameof(Domain.Entities.Product.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Product.Sold).ToLower())
                {
                    AddOrderByDescending(x => x.Sold);
                }
                else if (columnName == nameof(Domain.Entities.Product.Quantity).ToLower())
                {
                    AddOrderByDescending(x => x.Quantity);
                }
                else if (columnName == nameof(Domain.Entities.Product.ActualInventory).ToLower())
                {
                    AddOrderByDescending(x => x.ActualInventory);
                }
                else if (columnName == nameof(Domain.Entities.Product.Code).ToLower())
                {
                    AddOrderByDescending(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.Product.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else if (columnName == nameof(Domain.Entities.Product.Category.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Category.Name);
                }
                else if (columnName == nameof(Domain.Entities.Product.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Product.UpdatedAt).ToLower())
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