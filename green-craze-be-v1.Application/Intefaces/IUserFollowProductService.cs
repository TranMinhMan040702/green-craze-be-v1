using green_craze_be_v1.Application.Model.UserFollowProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUserFollowProductService
    {
        Task<bool> LikeProduct(FollowProductRequest request);
        Task<bool> UnLikeProduct(FollowProductRequest request);
    }
}
