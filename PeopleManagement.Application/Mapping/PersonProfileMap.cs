using AutoMapper;
using Newtonsoft.Json;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Domain.Entities;
using System.Text.RegularExpressions;

namespace PeopleManagement.Application.Mapping
{
    public class PersonProfileMap : Profile
    {
        public PersonProfileMap()
        {
            CreateMap<Person, PersonDto>()
                .ReverseMap();

            //CreateMap<Person, PersonDto>()
            //    .ReverseMap()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            //    .ForMember(dest => dest.DeletionAt, opt => opt.Ignore())
            //    .ForMember(dest => dest.CPF, opt => opt.MapFrom(src =>
            //        src.CPF != null ? Regex.Replace(src.CPF, @"\D", "") : src.CPF));

            //CreateMap<string, FilterCriteriaDto>().ConvertUsing((src, dest, context) =>
            //    {
            //        if (string.IsNullOrWhiteSpace(src))
            //            return new FilterCriteriaDto();

            //            return JsonConvert.DeserializeObject<FilterCriteriaDto>(src) ?? new FilterCriteriaDto();
            //    });
        }
    }
}