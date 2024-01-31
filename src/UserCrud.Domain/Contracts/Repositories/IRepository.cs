using UserCrud.Domain.Models;

namespace UserCrud.Domain.Contracts.Repositories;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}