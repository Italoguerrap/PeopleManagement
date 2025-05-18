using FluentValidation;
using PeopleManagement.API.Requests;

namespace PeopleManagement.API.Validations;

public class CreatePersonRequestValidator : BasePersonValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .MaximumLength(256)
           .WithMessage("Name must be at most 256 characters long");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .Must(CpfValidator.IsCpfValid)
            .WithMessage("CPF inválido");

        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddYears(-150)))
            .WithMessage("Too old");
    }
}
