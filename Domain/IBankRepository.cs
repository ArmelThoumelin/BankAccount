using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBankRepository
    {
        Task<TransactionResult> AddTransaction<T>(TransactionDemand<T> transactionDemand) where T : IAmount;
        Task<decimal> GetBalance(long IdAccount);
        Task<List<Transaction>> GetTransactions(HistoryDemand historyDemand);
        Task CheckAccount(long IdAccount);
    }

}
