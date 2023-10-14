using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IReviewService
    {
        Task<PaginatedResult<ReviewDto>> GetListReview(GetReviewPagingRequest request);

        Task<ReviewDto> GetReview(long id);

        Task<long> CreateReview(CreateReviewRequest request);

        Task<bool> ReplyReview(long id, ReplyReviewRequest request);

        Task<bool> DeleteReview(long id);

        Task<bool> DeleteListReview(List<long> ids);
    }
}
