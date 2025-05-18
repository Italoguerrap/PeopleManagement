using PeopleManagement.Application.DTOs;
using PeopleManagement.Domain.Entities;

namespace PeopleManagement.Application.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person?> GetByCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<List<Person>> QueryAsync(FilterCriteriaDto query, CancellationToken cancellationToken);
    }
}