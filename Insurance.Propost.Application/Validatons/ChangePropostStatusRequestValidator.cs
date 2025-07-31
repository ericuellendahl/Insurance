using FluentValidation;
using Insurance.Propost.Application.DTOs;

namespace Insurance.Propost.Application.Validatons
{
    public class ChangePropostStatusRequestValidator : AbstractValidator<ChangePropostStatusRequest>
    {
        public ChangePropostStatusRequestValidator()
        {
            RuleFor(x => x.PropostId)
                .NotEmpty()
                .WithMessage("Proposal ID is required");

            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("Status must be valid");
        }
    }
}
