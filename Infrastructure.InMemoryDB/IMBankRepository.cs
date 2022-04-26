using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.InMemoryDB
{
    public class IMBankRepository : IBankRepository
    {
        protected IMDBContext context { get => ContextInstance.GetInstance(); }

        public async Task<TransactionResult> AddTransaction(TransactionDemand transactionDemand)
        {
            await CheckAccount(transactionDemand.IdAccount);
            context.Set<DBModels.Transaction>().Add(new DBModels.Transaction() { IdAccount = transactionDemand.IdAccount, Amount = transactionDemand.Amount, TransactionDate = transactionDemand.TransactionDate });
            var result = new TransactionResult();
            if (await context.SaveChangesAsync() == 1)
            {
                result.Result = TransactionResult.TransactionStatus.Ok;
            }
            else
            {
                throw new Domain.BankException.TransactionErrorException();
            }

            return result;
        }

        private async Task CheckAccount(long IdAccount)
        {
            if (! await AccountExists(IdAccount))
            {
                throw new Domain.BankException.InvalidAccountException();
            }
        }

        private async Task<bool> AccountExists(long IdAccount)
        {
            return await context.Set<DBModels.Account>().AnyAsync();
        }
    }
}
