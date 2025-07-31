using FluentValidation;
using Insurance.Hiring.Application.DTOs;

namespace Insurance.Hiring.Application.Validations
{
    public class ContractProposalRequestValidator : AbstractValidator<ContractPropostRequest>
    {
        public ContractProposalRequestValidator()
        {
            RuleFor(x => x.PropostId)
                .NotEmpty()
                .WithMessage("Proposal ID is required.");
        }
    }
}
