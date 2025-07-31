using FluentValidation;
using Insurance.Propost.Application.DTOs;

namespace Insurance.Propost.Application.Validatons
{
    public class CreatePropostRequestValidator : AbstractValidator<CreatePropostRequest>
    {
        public CreatePropostRequestValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .WithMessage("Customer name is required")
                .MaximumLength(200)
                .WithMessage("Customer name must be at most 200 characters long");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be in a valid format");

            RuleFor(x => x.CoverageAmount)
                .GreaterThan(0)
                .WithMessage("Coverage amount must be greater than zero");

            RuleFor(x => x.InsuranceType)
                .NotEmpty()
                .WithMessage("Insurance type is required");
        }
    }
}
