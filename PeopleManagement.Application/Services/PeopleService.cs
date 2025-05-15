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
                people = await personRepository.GetAllAsync(cancellationToken);
            else
                people = await personRepository.SearchAsync(query, cancellationToken);

            return [.. people.Select(mapper.Map<PersonDto>)];
        }

        public async Task AddAsync(PersonDto personDto, CancellationToken cancellationToken)
        {
            if (personDto == null)
                throw new ArgumentNullException(nameof(personDto), "Person data is required.");

            Person person = mapper.Map<Person>(personDto);

            await personRepository.AddAsync(person, cancellationToken);
        }

        public async Task<PersonDto> UpdateAsync(long id, PersonDto personDto, CancellationToken cancellationToken)
        {
            Person person = await personRepository.UpdateAsync(id, mapper.Map<Person>(personDto), cancellationToken);

            return mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            Person person = await personRepository.GetByIdAsync(id, cancellationToken);

            if (person == null)
                throw new InvalidOperationException("Pessoa não encontrada.");

            await personRepository.DeleteAsync(id, cancellationToken);

            return mapper.Map<PersonDto>(person);
        }
    }
}