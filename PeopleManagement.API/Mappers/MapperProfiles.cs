using AutoMapper;
using PeopleManagement.API.Requests;
using PeopleManagement.Application.DTOs;

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
                .ForMember(dest => dest.DateOfBirth, o => o.MapFrom(src => src.DateOfBirth == null ? default : src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)));
        }
    }
}