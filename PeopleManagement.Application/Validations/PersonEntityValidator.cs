

//namespace PeopleManagement.Application.Validations
//{
//    public class PersonEntityValidator : AbstractValidator<Person>
//    {
//        private readonly IPersonRepository _personRepository;

//        public PersonEntityValidator(IPersonRepository personRepository)
//        {
//            _personRepository = personRepository;

//            RuleFor(p => p.Name)
//                .NotEmpty().WithMessage(PersonErrorMessages.NameRequired)
//                .MaximumLength(255).WithMessage(PersonErrorMessages.NameMaximumLength)
//                .Matches(@"^[a-zA-ZÀ-ú\s'-]+$").WithMessage(PersonErrorMessages.NameCharacterValidation);

//            RuleFor(p => p.Email)
//                .EmailAddress().WithMessage(PersonErrorMessages.InvalidEmail)
//                .When(p => !string.IsNullOrWhiteSpace(p.Email));

//            RuleFor(p => p.DateOfBirth)
//                .NotEmpty().WithMessage(PersonErrorMessages.DateOfBirthRequired)
//                .Must(BeAValidDate).WithMessage(PersonErrorMessages.DateOfBirthInvalid)
//                .LessThanOrEqualTo(DateTime.Today).WithMessage(PersonErrorMessages.DateOfBirthInFuture);

//            RuleFor(p => p.CPF)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(PersonErrorMessages.CPFRequired)
//                .Must(IsCpfValid).WithMessage(PersonErrorMessages.CPFInvalid)
//                .MustAsync((person, cpf, context, cancellationToken) => BeUniqueCpf(person, cpf, cancellationToken))
//                .WithMessage(PersonErrorMessages.CPFDuplicate)
//                .When(_ => _personRepository != null);
//        }

//        private bool BeAValidDate(DateTime date)
//        {
//            return date != default && date > new DateTime(1900, 1, 1);
//        }

//        private async Task<bool> BeUniqueCpf(Person person, string cpf, CancellationToken cancellationToken)
//        {
//            if (string.IsNullOrWhiteSpace(cpf) || _personRepository == null)
//                return true;

//            cpf = Regex.Replace(cpf, @"\D", "");

//            Person existingPerson = await _personRepository.GetByCpfAsync(cpf, cancellationToken);

//            return existingPerson == null || existingPerson.Id == person.Id;
//        }

//        private bool IsCpfValid(string? cpf)
//        {
//            if (string.IsNullOrWhiteSpace(cpf))
//                return false;

//            cpf = Regex.Replace(cpf, @"\D", "");

//            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
//                return false;

//            string baseCpf = cpf[..9];
//            string firstCheckDigit = CalculateCheckDigit(baseCpf, new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
//            string secondCheckDigit = CalculateCheckDigit(baseCpf + firstCheckDigit, new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

//            return cpf.EndsWith(firstCheckDigit + secondCheckDigit);
//        }

//        private static string CalculateCheckDigit(string baseCpf, int[] multipliers)
//        {
//            int sum = baseCpf
//                .Select((digit, index) => int.Parse(digit.ToString()) * multipliers[index])
//                .Sum();

//            int remainder = sum % 11;
//            return (remainder < 2 ? 0 : 11 - remainder).ToString();
//        }
//    }
//}