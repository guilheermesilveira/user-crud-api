using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Contracts;
using UserCrud.Domain.Models;

namespace UserCrud.Infra.Data.Context;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }
}