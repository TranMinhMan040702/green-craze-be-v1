using green_craze_be_v1.Application.Model.Role;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.Role
{
    public class RoleSpecification : BaseSpecification<AppRole>
    {
        public RoleSpecification(string name) : base(x => x.NormalizedName.ToLower() == name)
        {
        }

        public RoleSpecification(GetRolePagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.Contains(keyword);
            }
            if (request.IsSortAscending)
            {
                if (request.ColumnName == nameof(Domain.Entities.AppRole.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.AppRole.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (request.ColumnName == nameof(Domain.Entities.AppRole.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (request.ColumnName == nameof(Domain.Entities.AppRole.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}