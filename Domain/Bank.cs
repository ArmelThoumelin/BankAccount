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

        #region Deposit
        public async Task<TransactionResult> AddDeposit(DepositDemand depositDemand)
        {
            TransactionResult result;
            try
            {
                result = await _bankRepository.AddTransaction(depositDemand);
            }
            catch (BankException.InvalidAccountException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.Invalid, Message = BankMessages.UnknownAccount };
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
        #endregion


        #region Withdrawal
        public async Task<TransactionResult> AddWithdrawal(WithdrawalDemand withdrawalDemand)
        {
            TransactionResult result;
            try
            {
                await CheckForSufficientAmount(withdrawalDemand.IdAccount, withdrawalDemand.Amount);
                result = await _bankRepository.AddTransaction(withdrawalDemand);
            }
            catch (BankException.InvalidAccountException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.UnknownAccount, Message = BankMessages.UnknownAccount };
            }
            catch (BankException.InsufficientFundsException)
            {
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.InsufficientFunds, Message = BankMessages.InsufficientFunds };
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

        private async Task CheckForSufficientAmount(long idAccount, WithdrawalAmount withdrawalAmount)
        {
            decimal balance = await _bankRepository.GetBalance(idAccount);
            if (balance < withdrawalAmount)
            {
                throw new BankException.InsufficientFundsException();
            }
        }
        #endregion
    }
}
