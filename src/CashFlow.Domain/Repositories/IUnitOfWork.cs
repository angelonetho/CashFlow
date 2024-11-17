namespace CashFlow.Infrastructure.DataAccess.Repositories;

public interface IUnitOfWork
{
    void Commit();
}