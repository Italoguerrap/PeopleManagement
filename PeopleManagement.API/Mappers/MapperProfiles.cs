using AutoMapper;
using PeopleManagement.API.Requests;
using PeopleManagement.Application.DTOs;
using System.Text.RegularExpressions;

namespace PeopleManagement.API.Mappers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<GetPeopleQueryRequest, FilterCriteriaDto>();

            CreateMap<CreatePersonRequest, PersonDto>()
                .ForMember(dest => dest.DateOfBirth, o => o.MapFrom(src => src.DateOfBirth == null ? default : src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)));

            CreateMap<UpdatePersonRequest, PersonDto>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth == null ? default : src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.CPF) ? null : Regex.Replace(src.CPF, @"\D", "")));
        }
    }
}