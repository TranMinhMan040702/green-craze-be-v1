using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Specification.Unit
{
    public class UnitSpecification : BaseSpecification<Domain.Entities.Unit>
    {
        public UnitSpecification(GetUnitPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                if (query.Status)
                    Criteria = x => x.Name.Contains(keyword) && x.Status == true;
                else
                    Criteria = x => x.Name.Contains(keyword);
            }
            else
            {
                if (query.Status)
                    Criteria = x => x.Status == true;
                else
                    Criteria = x => true;
            }
            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Unit.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Unit.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Unit.UpdatedAt).ToLower())
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Unit.Status).ToLower())
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
                if (columnName == nameof(Domain.Entities.Unit.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.Unit.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Unit.UpdatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Unit.Status).ToLower())
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