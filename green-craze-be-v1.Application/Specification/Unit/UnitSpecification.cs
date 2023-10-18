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
                Criteria = x => x.Name.ToLower().Contains(keyword);
            }
            if (query.IsSortAccending)
            {
                if (query.ColumnName == nameof(Domain.Entities.Unit.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.Status).ToLower())
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
                if (query.ColumnName == nameof(Domain.Entities.Unit.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.Status).ToLower())
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