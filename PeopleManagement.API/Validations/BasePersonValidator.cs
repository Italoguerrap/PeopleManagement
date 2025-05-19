using FluentValidation;
using PeopleManagement.API.Requests;
using PeopleManagement.API.Resources;
using PeopleManagement.Domain.Enums;

namespace PeopleManagement.API.Validations;

public class BasePersonValidator<T> : AbstractValidator<T> where T : PersonRequest
{
    public BasePersonValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256)
            .WithMessage("Name must be at most 256 characters long")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddYears(-150)))
            .WithMessage("Date of birth must be a realistic value")
            .When(p => p.DateOfBirth is not null);


        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF inválido")
            .Must(CpfValidator.IsCpfValid).WithMessage("CPF inválido")
            .When(x => !string.IsNullOrWhiteSpace(x.CPF));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email inválido")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Gender)
            .Must(g => Enum.TryParse<GenderType>(g, true, out _))
            .WithMessage("Gender must be 'Male', 'Female', or 'Other'")
            .When(x => !string.IsNullOrWhiteSpace(x.Gender));

        RuleFor(x => x.Naturality)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Naturality));

        RuleFor(x => x.Nationality)
            .MaximumLength(100)
            .Must(n => Countries.CountryNames
                .Any(c => string.Equals(c, n, StringComparison.OrdinalIgnoreCase)))
            .WithMessage("País inválido")
            .When(x => !string.IsNullOrWhiteSpace(x.Nationality));

        RuleSet("v1", () => { });

        RuleSet("v2", () =>
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("O endereço não pode ser vazio")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
        });
    }
}
