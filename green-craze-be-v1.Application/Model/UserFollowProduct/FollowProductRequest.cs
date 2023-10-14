using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.UserFollowProduct
{
    public class FollowProductRequest
    {
        public string UserId { get; set; }
        public long ProductId { get; set; }
    }
}
