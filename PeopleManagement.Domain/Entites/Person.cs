using PeopleManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PeopleManagement.Domain.Entites
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }

        public GenderType Gender { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string? Naturality { get; set; }

        public string? Country { get; set; }

        public string? CPF { get; set; }
    }
}