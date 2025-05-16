using AutoMapper;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Domain.Entites;
using System.Text.RegularExpressions;

namespace PeopleManagement.Application.Mapping
{
    public class PersonProfileMap : Profile
    {
        public PersonProfileMap()
        {
            CreateMap<Person, PersonDto>();

            CreateMap<Person, PersonDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionAt, opt => opt.Ignore())
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src =>
                    src.CPF != null ? Regex.Replace(src.CPF, @"\D", "") : src.CPF));
        }
    }
}