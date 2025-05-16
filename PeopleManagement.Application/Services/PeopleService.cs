using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Application.Validations;
using PeopleManagement.Domain.Entites;

namespace PeopleManagement.Application.Services
{
    public class PeopleService(IPersonRepository personRepository, IMapper mapper, PersonEntityValidator _validator ) : IPeopleService
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
                throw new ArgumentNullException(nameof(personDto), "Dados da pessoa são obrigatórios.");

            Person person = mapper.Map<Person>(personDto);

            await ValidatePersonAsync(person, cancellationToken);

            await personRepository.AddAsync(person, cancellationToken);
        }
        
        public async Task<PersonDto> UpdateAsync(long id, PersonDto personDto, CancellationToken cancellationToken)
        {
            if (personDto == null)
                throw new ArgumentNullException(nameof(personDto), "Dados da pessoa são obrigatórios.");

            Person person = await personRepository.GetByIdAsync(id, cancellationToken);
            
            if (person == null)
                throw new InvalidOperationException($"Pessoa não encontrada.");

            person = mapper.Map<Person>(personDto);
            person.Id = id;

            await ValidatePersonAsync(person, cancellationToken);

            person = await personRepository.UpdateAsync(id, person, cancellationToken);

            return mapper.Map<PersonDto>(person);
        }
        
        public async Task<PersonDto> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            Person person = await personRepository.GetByIdAsync(id, cancellationToken);

            if (person == null)
                throw new InvalidOperationException($"Pessoa não encontrada.");

            bool deleted = await personRepository.DeleteAsync(id, cancellationToken);
            
            if (!deleted)
                throw new InvalidOperationException($"Não foi possível excluir a pessoa.");

            return mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            Person person = await personRepository.GetByIdAsync(id, cancellationToken);

            if (person == null)
                throw new InvalidOperationException($"Pessoa não encontrada.");

            return mapper.Map<PersonDto>(person);
        }

        private async Task ValidatePersonAsync(Person person, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(person, cancellationToken);
            
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}