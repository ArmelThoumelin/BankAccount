using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.InMemoryDB
{
    public class IMBankRepository : IBankRepository
    {
        protected IMDBContext context { get => ContextInstance.GetInstance(); }

        public async Task<TransactionResult> AddTransaction<T>(TransactionDemand<T> transactionDemand) where T : IAmount
        {
            await CheckAccount(transactionDemand.IdAccount);
            context.Set<DBModels.Transaction>().Add(new DBModels.Transaction() { IdAccount = transactionDemand.IdAccount, Amount = transactionDemand.Amount.Value, TransactionDate = transactionDemand.TransactionDate });
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

        public async Task<decimal> GetBalance(long IdAccount)
        {
            await CheckAccount(IdAccount);
            decimal result = await context.Set<DBModels.Transaction>().Where(t => t.IdAccount == IdAccount).SumAsync(t => t.Amount);            

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
            return await context.Set<DBModels.Account>().AnyAsync(a => a.Id == IdAccount);
        }
    }
}
