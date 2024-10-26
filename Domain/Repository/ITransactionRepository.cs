using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface ITransactionRepository : IRepositoryBase<Transaction, Guid>
    {

    }
}
