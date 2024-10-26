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

        public Task<PagedList<WalletTransaction>> GetAllWalletTransactionAsync(WalletTransactionParameter walletTransactionParameter)
        {
            try
            {
                var query = _context.WalletTransactions.AsNoTracking();

                SearchByName(ref query, walletTransactionParameter.Title);
                ApplySort(ref query, walletTransactionParameter.OrderBy);

                return PagedList<WalletTransaction>.ToPagedList(query, walletTransactionParameter.PageNumber, walletTransactionParameter.PageSize);
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
    }
}
