using green_craze_be_v1.Application.Model.Review;

namespace green_craze_be_v1.Application.Specification.Review
{
    public class ReviewSpecification : BaseSpecification<Domain.Entities.Review>
    {
        public ReviewSpecification(int limit)
        {
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(limit, 0);
            AddInclude(x => x.Product);
        }

        public ReviewSpecification(DateTime firstDate, DateTime lastDate, int rating)
            : base(x => x.CreatedAt >= firstDate && x.CreatedAt <= lastDate && x.Rating == rating)
        { }

        public ReviewSpecification(GetReviewPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;
            AddInclude(x => x.Product);
            AddInclude(x => x.User);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Variant);
            if (!string.IsNullOrEmpty(keyword))
            {
                if (query.ProductId == null)
                {
                    Criteria = x => x.Title.Contains(keyword) || x.Product.Name.Contains(keyword);
                }
                else
                {
                    Criteria = x => (x.Title.Contains(keyword) || x.Product.Name.Contains(keyword)) && x.Product.Id == query.ProductId;
                }
            }
            else
            {
                if (query.ProductId == null)
                {
                    Criteria = x => true;
                }
                else
                {
                    if (query.Status)
                    {
                        if(query.Rating != null)
                        {
                            Criteria = x => x.Product.Id == query.ProductId && x.Status == true && x.Rating == query.Rating;
                        }
                        else
                        {
                            Criteria = x => x.Product.Id == query.ProductId && x.Status == true;
                        }
                    }
                    else
                    {
                        Criteria = x => x.Product.Id == query.ProductId;
                    }
                }
            }
            var columnName = query.ColumnName.ToLower();
            if (query.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Review.Title).ToLower())
                {
                    AddOrderBy(x => x.Title);
                }
                else if (columnName == nameof(Domain.Entities.Review.Id).ToLower())
                {
                    AddOrderBy(x => x.Id);
                }
                else if (columnName == nameof(Domain.Entities.Review.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Review.Product.Name).ToLower())
                {
                    AddOrderBy(x => x.Product.Name);
                }
                else if (columnName == nameof(Domain.Entities.Review.Rating).ToLower())
                {
                    AddOrderBy(x => x.Rating);
                }
                else if (columnName == nameof(Domain.Entities.Review.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.Review.Title).ToLower())
                {
                    AddOrderByDescending(x => x.Title);
                }
                else if (columnName == nameof(Domain.Entities.Review.Id).ToLower())
                {
                    AddOrderByDescending(x => x.Id);
                }
                else if (columnName == nameof(Domain.Entities.Review.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Review.Product.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Product.Name);
                }
                else if (columnName == nameof(Domain.Entities.Review.Rating).ToLower())
                {
                    AddOrderByDescending(x => x.Rating);
                }
                else if (columnName == nameof(Domain.Entities.Review.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
            }
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }

        public ReviewSpecification(string userId, long id) : base(x => x.User.Id == userId && x.Id == id)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Variant);
        }

        public ReviewSpecification(long orderItemId, string userId) : base(x => x.OrderItem.Id == orderItemId && x.User.Id == userId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Variant);
        }

        public ReviewSpecification(long orderId) : base(x => x.OrderItem.Order.Id == orderId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Order);
        }

        public ReviewSpecification(long productId, bool status = true) : base(x => x.Product.Id == productId && x.Status == status)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Variant);
        }

        public ReviewSpecification(bool status, long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
            AddInclude(x => x.OrderItem);
            AddInclude(x => x.OrderItem.Variant);
        }
    }
}