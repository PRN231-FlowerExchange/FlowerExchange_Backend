using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class TransactionRepository : RepositoryBase<Transaction, Guid>, ITransactionRepository
    {
        private readonly FlowerExchangeDbContext _context;

        public TransactionRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }
    }
}
