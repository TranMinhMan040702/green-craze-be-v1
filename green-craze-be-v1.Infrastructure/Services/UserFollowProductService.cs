using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.UserFollowProduct;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class UserFollowProductService : IUserFollowProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserFollowProductService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> LikeProduct(FollowProductRequest request)
        {
            UserFollowProduct userFollowProduct = new()
            {
                Product = await _unitOfWork.Repository<Product>().GetById(request.ProductId),
                User = await _unitOfWork.Repository<AppUser>().GetById(request.UserId)
            };
            await _unitOfWork.Repository<UserFollowProduct>().Insert(userFollowProduct);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entities");
            }

            return isSuccess;
        }
    }
}
