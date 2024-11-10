using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Persistence.RepositoryAdapter
{
    public class WalletTransactionRepository : RepositoryBase<WalletTransaction, Guid>, IWalletTransactionRepository
    {
        private readonly FlowerExchangeDbContext _context;
        public WalletTransactionRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public Task<PagedList<Transaction>> GetAllWalletTransactionAsync(WalletTransactionParameter walletTransactionParameter)
        {
            try
            {
                var query = _context.WalletTransactions
                    .Include(wt => wt.Transaction)
                    .AsNoTracking();

                SearchByName(ref query, walletTransactionParameter.Title);
                ApplySort(ref query, walletTransactionParameter.OrderBy);

                var transactionQuery = query.Select(wt => wt.Transaction);

                return PagedList<Domain.Entities.Transaction>.ToPagedList(transactionQuery, walletTransactionParameter.PageNumber, walletTransactionParameter.PageSize);
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                throw;
            }
        }

        private void SearchByName(ref IQueryable<WalletTransaction> walletTransactions, string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return;

            walletTransactions = walletTransactions.Where(o => o.TransactonId.ToString().ToLower().Contains(title.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<WalletTransaction> walletTransactions, string orderByQueryString)
        {
            if (!walletTransactions.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                walletTransactions = walletTransactions.OrderBy(x => x.CreatedAt);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(WalletTransaction).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                walletTransactions = walletTransactions.OrderBy(x => x.CreatedAt);
                return;
            }
            walletTransactions = walletTransactions.OrderBy(orderQuery);
        }
        
        public async Task<PagedList<WalletTransaction>> GetWalletTransactionsByWalletIdAsync(Guid walletId, WalletTransactionParameter walletTransactionParameter)
        {
            try
            {
                if (!_context.WalletTransactions.Any())
                    return new PagedList<WalletTransaction>();
                
                var query = _context.WalletTransactions
                    .Include(wt => wt.Transaction)
                    .ThenInclude(wt => wt.ServiceOrder)
                    .Include(wt => wt.Transaction)
                    .ThenInclude(wt => wt.FlowerOrder)
                    .Where(wt => wt.WalletId.Equals(walletId))
                    .AsNoTracking();

                SearchByName(ref query, walletTransactionParameter.Title);
                ApplySort(ref query, walletTransactionParameter.OrderBy);

                return await PagedList<WalletTransaction>.ToPagedList(query, walletTransactionParameter.PageNumber, walletTransactionParameter.PageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
