using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBank
    {
        Task<TransactionResult> AddDeposit(DepositDemand depositDemand);
        Task<TransactionResult> AddWithdrawal(WithdrawalDemand withdrawalDemand);
        Task<decimal> GetBalance(long idAccount);
        Task<HistoryResult> GetTransactions(HistoryDemand historyDemand);
        Task<AccountExistsResult> AccountExists(long IdAccount);
    }

}
