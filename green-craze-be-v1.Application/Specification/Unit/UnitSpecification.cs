using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.Unit
{
    public class UnitSpecification : BaseSpecification<Domain.Entities.Unit>
    {
        public UnitSpecification(GetUnitPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name == keyword;
            }

            if (query.IsSortAccending)
            {
                if (query.ColumnName == nameof(Domain.Entities.Unit.Name))
                {
                    AddOrderBy(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.CreatedAt))
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.UpdatedAt))
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
                if (query.ColumnName == nameof(Domain.Entities.Unit.Name))
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.CreatedAt))
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Unit.UpdatedAt))
                {
                    AddOrderByDescending(x => x.UpdatedAt);
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