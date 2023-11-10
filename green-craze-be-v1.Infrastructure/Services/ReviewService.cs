using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Specification.Review;
using green_craze_be_v1.Application.Specification.Unit;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<PaginatedResult<ReviewDto>> GetListReview(GetReviewPagingRequest request)
        {
            var spec = new ReviewSpecification(request, isPaging: true);
            var countSpec = new ReviewSpecification(request);

            var reviews = await _unitOfWork.Repository<Review>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Review>().CountAsync(countSpec);
            var reviewDtos = new List<ReviewDto>();
            reviews.ForEach(x => reviewDtos.Add(_mapper.Map<ReviewDto>(x)));

            return new PaginatedResult<ReviewDto>(reviewDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<List<ReviewDto>> GetTop5ReviewLatest()
        {
            var reviewDtos = new List<ReviewDto>();
            var reviews = await _unitOfWork.Repository<Review>().ListAsync(new ReviewSpecification(5));
            reviews.ForEach(x => reviewDtos.Add(_mapper.Map<ReviewDto>(x)));
            return reviewDtos;
        }

        public async Task<ReviewDto> GetReview(long id)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(id);

            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<long> CreateReview(CreateReviewRequest request)
        {
            var review = _mapper.Map<Review>(request);
            review.Image = _uploadService.UploadFile(request.Image).Result;
            await _unitOfWork.Repository<Review>().Insert(review);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            return review.Id;
        }

        public async Task<bool> ReplyReview(long id, ReplyReviewRequest request)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(id);
            review.Reply = request.Reply;
            _unitOfWork.Repository<Review>().Update(review);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteReview(long id)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(id);

            review.Status = false;
            _unitOfWork.Repository<Review>().Update(review);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListReview(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var review = await _unitOfWork.Repository<Review>().GetById(id);
                    review.Status = false;
                    _unitOfWork.Repository<Review>().Update(review);
                }

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
                }
                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

    }
}
