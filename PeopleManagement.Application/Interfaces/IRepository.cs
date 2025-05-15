using PeopleManagement.Domain.Entites;

namespace PeopleManagement.Application.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<List<T>> SearchAsync(string query, CancellationToken cancellationToken);

        Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken);

        Task<T> UpdateAsync(long id, T entity, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    }
}