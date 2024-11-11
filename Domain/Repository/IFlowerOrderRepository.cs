using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;

namespace Domain.Repository;

public interface IFlowerOrderRepository : IRepositoryBase<FlowerOrder, Guid>
{
    Task<List<FlowerOrder>> GetFlowerOrdersByWalletIdAndTypeAndStatus(Guid walletId, TransactionType type, TransStatus status);
}