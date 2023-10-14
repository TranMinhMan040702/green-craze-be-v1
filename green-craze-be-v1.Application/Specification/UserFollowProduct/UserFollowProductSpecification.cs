using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.UserFollowProduct
{
    public class UserFollowProductSpecification : BaseSpecification<Domain.Entities.UserFollowProduct>
    {
        public UserFollowProductSpecification(string userId, long productId) 
            : base(x => x.User.Id == userId && x.Product.Id == productId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Product);
        }
    }
}
