using AutoMapper;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Domain.Entites;

namespace PeopleManagement.Application.Services
{
    public class PeopleService(IPersonRepository personRepository, IMapper mapper) : IPeopleService
    {
        public async Task<List<PersonDto>> SearchAsync(string query, CancellationToken cancellationToken)
        {
            List<Person> people;

            if (string.IsNullOrWhiteSpace(query))
            {
                people = await personRepository.GetAllAsync(cancellationToken);
            }
            else
                people = await personRepository.SearchAsync(query, cancellationToken);

            return [.. people.Select(mapper.Map<PersonDto>)];
        }
    }
}