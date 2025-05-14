using Microsoft.EntityFrameworkCore;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Domain.Entites;
using PeopleManagement.Infrastructure.Context;

namespace PeopleManagement.Infrastructure.Repositories
{
    public class PersonRepository(AppDbContext appDbContext) : IPersonRepository
    {
        public async Task<Person> AddAsync(Person entity, CancellationToken cancellationToken)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await appDbContext.People.AddAsync(entity, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var person = await appDbContext.People.FindAsync(new object[] { id }, cancellationToken);

            if (person == null)
                return false;

            appDbContext.People.Remove(person);
            await appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<Person>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.People.ToListAsync(cancellationToken);
        }

        public async Task<Person?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await appDbContext.People.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Person>> SearchAsync(string query, CancellationToken cancellationToken)
        {
            return await appDbContext.People
                .Where(p => p.Name.Contains(query) || p.Email.Contains(query))
                .ToListAsync(cancellationToken);
        }

        public async Task<Person> UpdateAsync(Person entity, CancellationToken cancellationToken)
        {
            var existing = await appDbContext.People.FindAsync(new object[] { entity.Id }, cancellationToken);

            if (existing == null)
                throw new InvalidOperationException("Pessoa não encontrada.");

            existing.Name = entity.Name;
            existing.Gender = entity.Gender;
            existing.Email = entity.Email;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.Naturality = entity.Naturality;
            existing.Country = entity.Country;
            existing.CPF = entity.CPF;
            existing.UpdatedAt = DateTime.UtcNow;

            appDbContext.People.Update(existing);
            await appDbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }
    }
}