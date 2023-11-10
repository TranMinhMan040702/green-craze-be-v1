using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Paging;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface INotificationService
    {
        Task<long> CreateNotification(CreateNotificationRequest request);

        Task<bool> UpdateNotification(UpdateNotificationRequest request);

        Task<PaginatedResult<NotificationDto>> GetListNotification(GetNotificationPagingRequest request);
    }
}