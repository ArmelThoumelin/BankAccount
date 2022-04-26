using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBankRepository
    {
        Task<TransactionResult> AddTransaction(TransactionDemand transactionDemand);
    }

}
