using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBankRepository
    {
        Task<TransactionResult> AddTransaction<T>(TransactionDemand<T> transactionDemand) where T : IAmount;
        Task<decimal> GetBalance(long IdAccount);
    }

}
