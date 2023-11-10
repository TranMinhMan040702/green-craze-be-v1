using green_craze_be_v1.Application.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Transaction
{
    public class TransactionSpecification : BaseSpecification<Domain.Entities.Transaction>
    {
        public TransactionSpecification() 
        {
            AddInclude(x => x.Order);
        }

        public TransactionSpecification(int limit)
        {
            AddInclude(x => x.Order);
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(limit, 0);
        }

        public TransactionSpecification(DateTime firstDate, DateTime lastDate) 
            : base(x => x.CreatedAt >= firstDate && x.CreatedAt <= lastDate)
        {
            AddInclude(x => x.Order);
        }

        public TransactionSpecification(GetTransactionPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            AddInclude(x => x.Order);
            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.PaymentMethod.ToLower().Contains(keyword)
                || x.Order.Code.ToLower().Contains(keyword);
            }
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAccending)
            {
                if (columnName == nameof(Domain.Entities.Transaction.PaymentMethod).ToLower())
                {
                    AddOrderBy(x => x.PaymentMethod);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.CompletedAt).ToLower())
                {
                    AddOrderBy(x => x.CompletedAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.PaidAt).ToLower())
                {
                    AddOrderBy(x => x.PaidAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.TotalPay).ToLower())
                {
                    AddOrderBy(x => x.TotalPay);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.Order.Code).ToLower())
                {
                    AddOrderBy(x => x.Order.Code);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.Transaction.PaymentMethod).ToLower())
                {
                    AddOrderByDescending(x => x.PaymentMethod);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.CompletedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CompletedAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.PaidAt).ToLower())
                {
                    AddOrderByDescending(x => x.PaidAt);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.TotalPay).ToLower())
                {
                    AddOrderByDescending(x => x.TotalPay);
                }
                else if (columnName == nameof(Domain.Entities.Transaction.Order.Code).ToLower())
                {
                    AddOrderByDescending(x => x.Order.Code);
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

        public TransactionSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Order);
        }
    }
}