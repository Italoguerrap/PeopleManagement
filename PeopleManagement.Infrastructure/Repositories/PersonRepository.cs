using Microsoft.EntityFrameworkCore;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Domain.Entities;
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
            Person? person = await appDbContext.People.FindAsync([id], cancellationToken);

            if (person == null)
                return false;

            person.DeletionAt = DateTime.Now;

            await appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<Person>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.People
                .Where(person => person.DeletionAt == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<Person?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await appDbContext.People
                .Where(person => person.DeletionAt == null)
                .FirstOrDefaultAsync(person => person.Id == id && person.DeletionAt == null, cancellationToken);
        }

        public async Task<List<Person>> QueryAsync(FilterCriteriaDto query, CancellationToken cancellationToken)
        {
            IQueryable<Person> queryable = appDbContext.People.Where(person => person.DeletionAt == null);

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryable = queryable.Where(person => person.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                queryable = queryable.Where(person => person.Email.Contains(query.Email));
            }

            if (query.DateOfBirth.HasValue)
            {
                var dateOfBirth = query.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local);
                queryable = queryable.Where(person => person.DateOfBirth == dateOfBirth);
            }

            if (!string.IsNullOrWhiteSpace(query.CPF))
            {
                queryable = queryable.Where(person => person.CPF == query.CPF);
            }

            if (!string.IsNullOrWhiteSpace(query.Naturality))
            {
                queryable = queryable.Where(person => person.Naturality.Contains(query.Naturality));
            }

            if (!string.IsNullOrWhiteSpace(query.Country))
            {
                queryable = queryable.Where(person => person.Country.Contains(query.Country));
            }

            if (query.Gender is not null)
            {
                queryable = queryable.Where(person => person.Gender == query.Gender);
            }

            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<Person> UpdateAsync(long id, Person entity, CancellationToken cancellationToken)
        {
            Person? existing = await appDbContext.People.FindAsync([id], cancellationToken) ?? throw new InvalidOperationException("Pessoa não encontrada.");

            existing.Name = entity.Name;
            existing.Gender = entity.Gender;
            existing.Email = entity.Email;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.Naturality = entity.Naturality;
            existing.Country = entity.Country;
            existing.CPF = entity.CPF;
            existing.UpdatedAt = DateTime.Now;

            await appDbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }

        public async Task<Person?> GetByCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            return await appDbContext.People.Where(person => person.DeletionAt == null).FirstOrDefaultAsync(p => p.CPF == cpf, cancellationToken);
        }
    }
}