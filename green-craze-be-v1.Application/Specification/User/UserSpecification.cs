using green_craze_be_v1.Application.Model.User;

namespace green_craze_be_v1.Application.Specification.User
{
    public class UserSpecification : BaseSpecification<Domain.Entities.AppUser>
    {
        public UserSpecification(GetUserPagingRequest request, bool isPaging = false)
        {
            var key = request.Search;
            if (!string.IsNullOrEmpty(key))
            {
                Criteria = x => x.Staff == null && x.Email.ToLower().Contains(key)
                || x.PhoneNumber.ToLower().Contains(key)
                || x.Gender.ToLower().Contains(key)
                || x.Dob.Value.ToString().Contains(key)
                || x.FirstName.ToLower().Contains(key)
                || x.LastName.ToLower().Contains(key);
            }
            else
            {
                Criteria = x => x.Staff == null;
            }
            AddInclude(x => x.Staff);
            var columnName = request.ColumnName;
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email))
                {
                    AddOrderBy(x => x.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt))
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.UpdatedAt))
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email))
                {
                    AddOrderByDescending(x => x.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt))
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.UpdatedAt))
                {
                    AddOrderByDescending(x => x.UpdatedAt);
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

        public UserSpecification(string userId) : base(x => x.Id == userId)
        {
            AddInclude(x => x.Addresses);
            AddInclude(x => x.Cart);
        }
    }
}