using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.User
{
    public class UpdateStaffRequest : UpdateUserRequest
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}