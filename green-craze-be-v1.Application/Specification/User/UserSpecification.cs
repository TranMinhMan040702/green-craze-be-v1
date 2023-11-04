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
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email).ToLower())
                {
                    AddOrderBy(x => x.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.FirstName).ToLower())
                {
                    AddOrderBy(x => x.FirstName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.LastName).ToLower())
                {
                    AddOrderBy(x => x.LastName);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email).ToLower())
                {
                    AddOrderByDescending(x => x.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.FirstName).ToLower())
                {
                    AddOrderByDescending(x => x.FirstName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.LastName).ToLower())
                {
                    AddOrderByDescending(x => x.LastName);
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
            AddInclude(x => x.AppUserTokens);
        }

        public UserSpecification()
        {
            AddInclude(x => x.AppUserTokens);
        }

        public UserSpecification(string email, bool test = false) : base(x => x.Email == email)
        {
            AddInclude(x => x.AppUserTokens);
        }
    }
}