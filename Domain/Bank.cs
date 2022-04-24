using Domain.Models;

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

        public TransactionResult AddDeposit(DepositDemand depositDemand)
        {
            return _bankRepository.AddTransaction(depositDemand);
        }
    }
}
