using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public class Bank : IBank
    {
        private readonly IBankRepository _bankRepository;

        /// <summary>
        /// Initialize a new Bank instance
        /// </summary>
        /// <param name="bankRepository">Bank repository</param>
        public Bank(IBankRepository bankRepository)
        {
            this._bankRepository = bankRepository;
        }

        public async Task<TransactionResult> AddDeposit(DepositDemand depositDemand)
        {
            TransactionResult result;
            if (!depositDemand.CheckAmount())
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Unauthorized, Message = "The amount can't be negative nor equal to zero" }; //TODO:Manage message
            }
            else
            {
                result = await _bankRepository.AddTransaction(depositDemand);
            }
            return result;
        }
    }
}
