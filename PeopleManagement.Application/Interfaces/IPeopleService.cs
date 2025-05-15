using PeopleManagement.Application.DTOs;

namespace PeopleManagement.Application.Interfaces
{
    public interface IPeopleService
    {
        Task<List<PersonDto>> SearchAsync(string query, CancellationToken cancellationToken);

        Task AddAsync(PersonDto personDto, CancellationToken cancellationToken);

        Task<PersonDto> UpdateAsync(long id, PersonDto personDto, CancellationToken cancellationToken);

        Task<PersonDto> DeleteAsync(long id, CancellationToken cancellationToken);
    }
}