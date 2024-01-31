using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Contracts;
using UserCrud.Domain.Contracts.Repositories;
using UserCrud.Domain.Models;
using UserCrud.Infra.Data.Context;

namespace UserCrud.Infra.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Create(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}