using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBank
    {
        Task<TransactionResult> AddDeposit(DepositDemand depositDemand);
        Task<TransactionResult> AddWithdrawal(WithdrawalDemand withdrawalDemand);
        Task<decimal> GetBalance(long idAccount);
        Task<List<Transaction>> GetTransactions(HistoryDemand historyDemand);
    }

}
