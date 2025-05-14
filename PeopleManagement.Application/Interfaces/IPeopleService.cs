using PeopleManagement.Application.DTOs;

namespace PeopleManagement.Application.Interfaces
{
    public interface IPeopleService
    {
        Task<List<PersonDto>> SearchAsync(string query, CancellationToken cancellationToken);
    }
}