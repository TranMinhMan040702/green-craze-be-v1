using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Docket
{
    public class DocketSpecification : BaseSpecification<Domain.Entities.Docket>
    {
        public DocketSpecification(long productId) : base(x => x.Product.Id == productId)
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.Product);
        }
    }
}
