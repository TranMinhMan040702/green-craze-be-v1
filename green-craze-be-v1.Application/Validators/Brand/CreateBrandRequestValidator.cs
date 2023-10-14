using FluentValidation;
using green_craze_be_v1.Application.Model.Brand;

namespace green_craze_be_v1.Application.Validators.Brand
{
    public class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequest>
    {
       public CreateBrandRequestValidator() 
       {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
       }
    }
}
