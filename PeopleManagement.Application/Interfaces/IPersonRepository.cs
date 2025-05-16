using PeopleManagement.Domain.Entites;

namespace PeopleManagement.Application.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person?> GetByCpfAsync(string cpf, CancellationToken cancellationToken);
    }
}