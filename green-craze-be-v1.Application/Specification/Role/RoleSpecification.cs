using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Role
{
    public class RoleSpecification : BaseSpecification<IdentityRole>
    {
        public RoleSpecification(string name) : base(x => x.NormalizedName.ToLower() == name)
        {
        }
    }
}