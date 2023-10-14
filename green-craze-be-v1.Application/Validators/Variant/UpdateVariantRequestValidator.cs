using FluentValidation;
using green_craze_be_v1.Application.Model.Variant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Variant
{
    public class UpdateVariantRequestValidator : AbstractValidator<UpdateVariantRequest>
    {
       public UpdateVariantRequestValidator() 
       {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Sku).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.ItemPrice).NotEmpty();
            RuleFor(x => x.TotalPrice).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
