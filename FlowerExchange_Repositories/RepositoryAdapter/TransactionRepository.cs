using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class TransactionRepository : RepositoryBase<Transaction, Guid>, ITransactionRepository
    {

        public TransactionRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
