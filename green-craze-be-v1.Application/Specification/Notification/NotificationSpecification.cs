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

            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Notification.Title).ToLower())
                {
                    AddOrderBy(x => x.Title);
                }
                else if (columnName == nameof(Domain.Entities.Notification.Content).ToLower())
                {
                    AddOrderBy(x => x.Content);
                }
                else if (columnName == nameof(Domain.Entities.Notification.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else
                {
                    AddOrderBy(x => x.UpdatedAt == null ? x.CreatedAt : x.UpdatedAt);
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
                else if (columnName == nameof(Domain.Entities.Notification.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
                }
                else
                {
                    AddOrderByDescending(x => x.UpdatedAt == null ? x.CreatedAt : x.UpdatedAt);
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
    }
}