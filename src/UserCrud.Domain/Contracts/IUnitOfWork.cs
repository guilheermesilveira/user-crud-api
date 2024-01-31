namespace UserCrud.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}