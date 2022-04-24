using Domain;
using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.InMemoryDB
{
    public class IMBankRepository : IBankRepository
    {
        public async Task<TransactionResult> AddTransaction(TransactionDemand transactionDemand)
        {
            throw new System.NotImplementedException();
        }
    }
}
