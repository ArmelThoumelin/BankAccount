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
                result = new TransactionResult() { Result = TransactionResult.TransactionStatus.UnknownAccount, Message = BankMessages.UnknownAccount };
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

        #region History
        public async Task<decimal> GetBalance(long idAccount)
        {
            return await _bankRepository.GetBalance(idAccount);
        }

        public async Task<HistoryResult> GetTransactions(HistoryDemand historyDemand)
        {
            var result = new HistoryResult();
            try
            {
                CheckDates(historyDemand);
                result.Transactions = await _bankRepository.GetTransactions(historyDemand);
                result.Result = HistoryResult.HistoryStatus.Ok;
            }
            catch (BankException.InvalidAccountException)
            {
                result = new HistoryResult() { Result = HistoryResult.HistoryStatus.UnknownAccount, Message = BankMessages.UnknownAccount };
            }
            catch (BankException.InvalidDateRangeException)
            {
                result = new HistoryResult() { Result = HistoryResult.HistoryStatus.InvalidDateRange, Message = BankMessages.HistoryDateRangeError };
            }
            catch (Exception)
            {
                result = new HistoryResult() { Result = HistoryResult.HistoryStatus.Invalid, Message = BankMessages.TransactionException };
            }
            return result;
        }

        private void CheckDates(HistoryDemand historyDemand)
        {
            if (historyDemand.StartDate > historyDemand.EndDate)
            {
                throw new BankException.InvalidDateRangeException();
            }
        }
        #endregion

        #region Account
        public async Task<AccountExistsResult> AccountExists(long IdAccount)
        {
            var result = new AccountExistsResult();
            try
            {
                await _bankRepository.CheckAccount(IdAccount);
                result.Result = AccountExistsResult.AccountStatus.Ok;
            }
            catch (BankException.InvalidAccountException)
            {
                result.Result = AccountExistsResult.AccountStatus.Ok;
                result.Message = BankMessages.UnknownAccount;
            }
            return result;
        }

        #endregion
    }
}
