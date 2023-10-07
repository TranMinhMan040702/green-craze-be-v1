using FluentValidation;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Unit
{
    public class UpdateUnitRequestValidator : AbstractValidator<UpdateUnitRequest>
    {
        public UpdateUnitRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}