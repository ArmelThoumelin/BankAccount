using Domain.Models;

namespace Domain
{
    public interface IBankRepository
    {
        TransactionResult AddTransaction(TransactionDemand transactionDemand);
    }

}
