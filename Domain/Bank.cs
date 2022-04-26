using Domain.Models;
using System;
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
            try
            {
                if (!depositDemand.CheckAmount())
                {
                    throw new BankException.InvalidAmountException();
                }
                else
                {
                    result = await _bankRepository.AddTransaction(depositDemand);
                }
            }
            catch (BankException.InvalidAccountException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Invalid, Message = BankMessages.UnknownAccount };
            }
            catch (BankException.InvalidAmountException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Unauthorized, Message = BankMessages.AmountForbidNegativeAndZero };
            }
            catch (BankException.TransactionErrorException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Invalid, Message = BankMessages.TransactionInfrastructureError };
            }
            catch (Exception)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Invalid, Message = BankMessages.TransactionException };
            }

            return result;
        }
    }
}
