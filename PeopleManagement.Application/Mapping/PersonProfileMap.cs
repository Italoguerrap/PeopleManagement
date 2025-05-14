using AutoMapper;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Domain.Entites;

namespace PeopleManagement.Application.Mapping
{
    public class PersonProfileMap : Profile
    {
        public PersonProfileMap()
        {
            CreateMap<Person, PersonDto>();
        }
    }
}