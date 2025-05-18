using System.Text.RegularExpressions;

namespace PeopleManagement.API.Validations;

public static class CpfValidator
{
    public static bool IsCpfValid(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = Regex.Replace(cpf, @"\D", "");

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        string baseCpf = cpf[..9];
        string firstCheckDigit = CalculateCheckDigit(baseCpf, new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
        string secondCheckDigit = CalculateCheckDigit(baseCpf + firstCheckDigit, new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

        return cpf.EndsWith(firstCheckDigit + secondCheckDigit);
    }

    private static string CalculateCheckDigit(string baseCpf, int[] multipliers)
    {
        int sum = baseCpf
            .Select((digit, index) => int.Parse(digit.ToString()) * multipliers[index])
            .Sum();

        int remainder = sum % 11;
        return (remainder < 2 ? 0 : 11 - remainder).ToString();
    }
}
