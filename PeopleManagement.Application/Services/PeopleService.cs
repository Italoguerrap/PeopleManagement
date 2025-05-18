using AutoMapper;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Application.Mapping;
using PeopleManagement.Domain.Entities;

namespace PeopleManagement.Application.Services
{
    public class PeopleService(IPersonRepository personRepository, IMapper mapper) : IPeopleService
    {
        public async Task<List<PersonDto>> SearchAsync(FilterCriteriaDto query, CancellationToken cancellationToken)
        {
            try
            {
                List<Person> people = query.IsEmpty
                    ? await personRepository.GetAllAsync(cancellationToken)
                    : await personRepository.QueryAsync(query, cancellationToken);

                return [.. people.Select(mapper.Map<PersonDto>)];
            }
            catch (Exception ex)
            {
                throw new PeopleManagementException($"Erro ao processar a consulta: {ex.Message}", ex);
            }
        }

        public async Task AddAsync(PersonDto personDto, CancellationToken cancellationToken)
        {
            if (personDto is null)
                throw new PeopleManagementException("Dados da pessoa são obrigatórios.");

            Person? existingPerson = await personRepository.GetByCpfAsync(personDto.CPF, cancellationToken);

            if (existingPerson is not null)
            {
                throw new PeopleManagementException("Este CPF já está cadastrado");
            }

            Person person = mapper.Map<Person>(personDto);

            await personRepository.AddAsync(person, cancellationToken);
        }

        public async Task<PersonDto> UpdateAsync(string cpf, PersonDto personDto, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cpf);

            if (personDto is null)
                throw new PeopleManagementException("Dados da pessoa são obrigatórios.");

            Person person = await personRepository.GetByCpfAsync(cpf, cancellationToken) ?? throw new PeopleManagementException($"Pessoa não encontrada.");

            long personId = person.Id;
            personDto.MergeInto(person);
            person = mapper.Map<Person>(personDto);
            person.Id = personId;

            if (cpf != personDto.CPF)
            {
                Person? existingPerson = await personRepository.GetByCpfAsync(personDto.CPF, cancellationToken);
                if (existingPerson is not null)
                {
                    throw new PeopleManagementException("Este CPF já está cadastrado");
                }
            }

            person = await personRepository.UpdateAsync(personId, person, cancellationToken);

            return mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> DeleteAsync(string cpf, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cpf);

            Person person = await personRepository.GetByCpfAsync(cpf, cancellationToken) ?? throw new PeopleManagementException($"Pessoa não encontrada.");

            bool deleted = await personRepository.DeleteAsync(person.Id, cancellationToken);

            if (!deleted)
                throw new PeopleManagementException($"Não foi possível excluir a pessoa.");

            return mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> GetByIdAsync(string cpf, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cpf);

            Person? person = await personRepository.GetByCpfAsync(cpf, cancellationToken) ?? throw new PeopleManagementException($"Pessoa não encontrada.");

            return mapper.Map<PersonDto>(person);
        }
    }
}