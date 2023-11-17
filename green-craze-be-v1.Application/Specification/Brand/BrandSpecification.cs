using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Specification.Brand
{
    public class BrandSpecification : BaseSpecification<Domain.Entities.Brand>
    {
        public BrandSpecification(GetBrandPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                if (query.Status)
                    Criteria = x => (x.Name.Contains(keyword) || x.Code.Contains(keyword)) && x.Status == true;
                else
                    Criteria = x => x.Name.Contains(keyword) || x.Code.Contains(keyword);
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
                if (columnName == nameof(Domain.Entities.Brand.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Brand.Code).ToLower())
                {
                    AddOrderBy(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.Brand.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Brand.UpdatedAt).ToLower())
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Brand.Status).ToLower())
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
                if (columnName == nameof(Domain.Entities.Brand.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Brand.Code).ToLower())
                {
                    AddOrderByDescending(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.Brand.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Brand.UpdatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Brand.Status).ToLower())
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