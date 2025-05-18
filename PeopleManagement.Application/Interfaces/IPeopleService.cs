using PeopleManagement.Application.DTOs;

namespace PeopleManagement.Application.Interfaces
{
    public interface IPeopleService
    {
        Task<List<PersonDto>> SearchAsync(FilterCriteriaDto query, CancellationToken cancellationToken);

        Task AddAsync(PersonDto personDto, CancellationToken cancellationToken);

        Task<PersonDto> UpdateAsync(string cpf, PersonDto personDto, CancellationToken cancellationToken);

        Task<PersonDto> DeleteAsync(string cpf, CancellationToken cancellationToken);

        Task<PersonDto> GetByIdAsync(string cpf, CancellationToken cancellationToken);
    }
}