using Microsoft.EntityFrameworkCore;
using PeopleManagement.Domain.Entities;

namespace PeopleManagement.Infrastructure.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Person> People { get; set; } = null!;
    }
}