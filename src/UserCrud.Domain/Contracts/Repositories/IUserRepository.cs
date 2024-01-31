using UserCrud.Domain.Models;

namespace UserCrud.Domain.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    void Create(User user);
    void Update(User user);
    void Delete(User user);
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetAll();
}