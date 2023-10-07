using FluentValidation;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Unit
{
    public class CreateUnitRequestValidator : AbstractValidator<CreateUnitRequest>
    {
        public CreateUnitRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}