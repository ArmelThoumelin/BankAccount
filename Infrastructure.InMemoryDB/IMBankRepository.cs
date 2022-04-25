using Domain;
using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.InMemoryDB
{
    public class IMBankRepository : IBankRepository
    {
        public async Task<TransactionResult> AddTransaction(TransactionDemand transactionDemand)
        {
            try
            {
                var context = ContextInstance.GetInstance();
                context.Set<DBModels.Transaction>().Add(new DBModels.Transaction() { IdAccount = transactionDemand.IdAccount, Amount = transactionDemand.Amount, TransactionDate = transactionDemand.TransactionDate });
                var result = new TransactionResult();
                if (await context.SaveChangesAsync() == 1)
                {
                    result.Result = TransactionResult.TransactionStatus.Ok;
                }
                else
                {
                    result.Result = TransactionResult.TransactionStatus.Invalid;
                    result.Message = "Transaction haven't been performed, we are sorry for the inconvenience"; // TODO:Manage message
                }

                return result;
            }
            catch (System.Exception)
            {
                return new TransactionResult() { Result = TransactionResult.TransactionStatus.Invalid, Message = "We encountered an unexpected error, we are sorry for the inconvenience" }; // TODO:Manage message
                // TODO:Manage Exception
            }

        }
    }
}
