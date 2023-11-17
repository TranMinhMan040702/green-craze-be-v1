using green_craze_be_v1.Application.Model.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.PaymentMethod
{
    public class PaymentMethodSpecification : BaseSpecification<Domain.Entities.PaymentMethod>
    {
        public PaymentMethodSpecification(GetPaymentMethodPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.ToLower().Contains(keyword)
                || x.Code.ToLower().Contains(keyword);
            }
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAscending)
            {
                if (columnName == nameof(Domain.Entities.PaymentMethod.Name).ToLower())
                {
                    AddOrderBy(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.Code).ToLower())
                {
                    AddOrderBy(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.Status).ToLower())
                {
                    AddOrderBy(x => x.Status);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.PaymentMethod.Name).ToLower())
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.Code).ToLower())
                {
                    AddOrderByDescending(x => x.Code);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.PaymentMethod.Status).ToLower())
                {
                    AddOrderByDescending(x => x.Status);
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