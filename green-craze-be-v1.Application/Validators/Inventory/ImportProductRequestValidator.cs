using FluentValidation;
using green_craze_be_v1.Application.Model.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Inventory
{
    public class ImportProductRequestValidator : AbstractValidator<ImportProductRequest>
    {
        public ImportProductRequestValidator()
        {
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.ActualInventory).NotNull();
            RuleFor(x => x.ProductId).NotNull();
        }
    }
}
