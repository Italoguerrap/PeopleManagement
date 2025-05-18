using PeopleManagement.Domain.Enums;

namespace PeopleManagement.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }

        public GenderType Gender { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string? CPF { get; set; }
    }
}