using FluentValidation;
using green_craze_be_v1.Application.Model.Variant;

namespace green_craze_be_v1.Application.Validators.Variant
{
    public class CreateVariantRequestValidator : AbstractValidator<CreateVariantRequest>
    {
        public CreateVariantRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Sku).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.ItemPrice).NotEmpty();
            RuleFor(x => x.TotalPrice).NotEmpty();
            RuleFor(x => x.PromotionalItemPrice).NotEmpty();
        }
    }
}
