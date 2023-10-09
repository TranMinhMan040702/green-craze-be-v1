using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Entities
{
    public class Staff : BaseAuditableEntity<string>
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public AppUser User { get; set; }
    }
}