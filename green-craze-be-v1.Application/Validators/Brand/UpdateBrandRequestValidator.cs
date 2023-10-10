using FluentValidation;
using green_craze_be_v1.Application.Model.Brand;

namespace green_craze_be_v1.Application.Validators.Brand
{
    public class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequest>
    {
        public UpdateBrandRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
