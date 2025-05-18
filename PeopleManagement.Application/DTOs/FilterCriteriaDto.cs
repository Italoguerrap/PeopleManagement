using PeopleManagement.Domain.Enums;

namespace PeopleManagement.Application.DTOs
{
    public class FilterCriteriaDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public GenderType? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Naturality { get; set; }
        public string? Country { get; set; }
        public string? CPF { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(Name)
                    && string.IsNullOrWhiteSpace(Email)
                    && Gender is null
                    && DateOfBirth is null
                    && string.IsNullOrWhiteSpace(Naturality)
                    && string.IsNullOrWhiteSpace(Country)
                    && string.IsNullOrWhiteSpace(CPF);
            }
        }
    }
}