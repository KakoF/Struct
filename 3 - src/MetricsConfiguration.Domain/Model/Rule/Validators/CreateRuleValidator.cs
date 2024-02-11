using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsConfiguration.Domain.Model.Rule.Validators
{
    public class CreateRuleValidator : AbstractValidator<CreateRuleRequest>
    {
        public CreateRuleValidator()
        {
            RuleFor(a => a.Name)
               .NotEmpty().WithMessage("Nome não pode ser vazio")
               .MinimumLength(5).WithMessage("Nome deve ter ao menos 5 caracteres")
               .MaximumLength(255).WithMessage("Nome tem limite máximo de de 255 caracteres");
            RuleFor(a => a.Description)
              .MinimumLength(5).WithMessage("Descrição deve ter ao menos 5 caracteres")
              .MaximumLength(255).WithMessage("Descrição tem limite máximo de de 255 caracteres");
        }
    }
}