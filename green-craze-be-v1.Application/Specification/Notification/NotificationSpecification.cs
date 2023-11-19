using green_craze_be_v1.Application.Model.Notification;

namespace green_craze_be_v1.Application.Specification.Notification
{
    public class NotificationSpecification : BaseSpecification<green_craze_be_v1.Domain.Entities.Notification>
    {
        public NotificationSpecification(GetNotificationPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (string.IsNullOrEmpty(request.UserId))
                return;

            AddInclude(x => x.User);

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => (x.Title.ToLower().Contains(keyword)
                || x.Content.Contains(keyword)) && x.User.Id == request.UserId;
            }
            else
            {
                Criteria = x => x.User.Id == request.UserId;
            }

            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAscending)
            {
                if (columnName == nameof(Domain.Entities.Notification.Title).ToLower())
                {
                    AddOrderBy(x => x.Title);
                }
                else if (columnName == nameof(Domain.Entities.Notification.Content).ToLower())
                {
                    AddOrderBy(x => x.Content);
                }
                else if (columnName == nameof(Domain.Entities.Notification.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Notification.UpdatedAt).ToLower())
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Status);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.Notification.Title).ToLower())
                {
                    AddOrderByDescending(x => x.Title);
                }
                else if (columnName == nameof(Domain.Entities.Notification.Content).ToLower())
                {
                    AddOrderByDescending(x => x.Content);
                }
                else if (columnName == nameof(Domain.Entities.Notification.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Notification.UpdatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderByDescending(x => x.Status);
                }
            }

            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }

        public NotificationSpecification(string userId, long id) : base(x => x.Id == id && x.User.Id == userId)
        {
            AddInclude(x => x.User);
        }

        public NotificationSpecification(string userId) : base(x => x.User.Id == userId)
        {
            AddInclude(x => x.User);
        }
        public NotificationSpecification(string userId, bool status) : base(x => x.User.Id == userId && x.Status == status)
        {
            AddInclude(x => x.User);
        }
    }
}