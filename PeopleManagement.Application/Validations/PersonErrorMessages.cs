namespace PeopleManagement.Application.Validations
{
    public static class PersonErrorMessages
    {
        public const string NameRequired = "Nome é obrigatório.";
        public const string NameMaximumLength = "O nome pode ter no máximo 255 caracteres.";
        public const string NameCharacterValidation = "O nome deve conter apenas letras, espaços, apóstrofos ou hífens.";
        public const string InvalidEmail = "Formato de e-mail inválido.";
        public const string DateOfBirthRequired = "Data de nascimento é obrigatória.";
        public const string DateOfBirthInFuture = "Data de nascimento não pode ser no futuro.";
        public const string DateOfBirthInvalid = "Data de nascimento inválida.";
        public const string CPFRequired = "CPF é obrigatório.";
        public const string CPFInvalid = "CPF inválido.";
        public const string CPFDuplicate = "CPF já cadastrado no sistema.";
    }
}