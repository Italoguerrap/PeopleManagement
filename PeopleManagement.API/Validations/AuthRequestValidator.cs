using FluentValidation;
using PeopleManagement.API.Requests;

namespace PeopleManagement.API.Validations
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("CPF is required.")                
                .Must(CpfValidator.IsCpfValid).WithMessage("CPF inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");                
        }
    }
}
