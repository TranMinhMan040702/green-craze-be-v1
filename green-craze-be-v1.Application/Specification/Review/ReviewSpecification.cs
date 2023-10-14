using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Review
{
    public class ReviewSpecification : BaseSpecification<Domain.Entities.Review>
    {
        public ReviewSpecification(GetReviewPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Title == keyword;
            }

            if (query.IsSortAccending)
            {
                if (query.ColumnName == nameof(Domain.Entities.Review.Title))
                {
                    AddOrderBy(x => x.Title);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Review.CreatedAt))
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Review.UpdatedAt))
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
                if (query.ColumnName == nameof(Domain.Entities.Review.Title))
                {
                    AddOrderByDescending(x => x.Title);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Review.CreatedAt))
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Review.UpdatedAt))
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
