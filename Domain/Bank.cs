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
            _bankRepository = bankRepository ?? throw new ArgumentNullException(nameof(bankRepository));
        }

        #region Deposit
        public async Task<TransactionResult> AddDeposit(DepositDemand depositDemand)
        {
            return await ExecuteTransactionAsync(() => _bankRepository.AddTransaction(depositDemand));
        }
        #endregion

        #region Withdrawal
        public async Task<TransactionResult> AddWithdrawal(WithdrawalDemand withdrawalDemand)
        {
            return await ExecuteTransactionAsync(async () =>
            {
                await CheckForSufficientAmount(withdrawalDemand.IdAccount, withdrawalDemand.Amount);
                return await _bankRepository.AddTransaction(withdrawalDemand);
            });
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
                ValidateDateRange(historyDemand);
                result.Transactions = await _bankRepository.GetTransactions(historyDemand);
                result.Result = HistoryResult.HistoryStatus.Ok;
            }
            catch (BankException.InvalidAccountException)
            {
                result = CreateHistoryResult(HistoryResult.HistoryStatus.UnknownAccount, BankMessages.UnknownAccount);
            }
            catch (BankException.InvalidDateRangeException)
            {
                result = CreateHistoryResult(HistoryResult.HistoryStatus.InvalidDateRange, BankMessages.HistoryDateRangeError);
            }
            catch (Exception)
            {
                result = CreateHistoryResult(HistoryResult.HistoryStatus.Invalid, BankMessages.TransactionException);
            }
            return result;
        }

        private void ValidateDateRange(HistoryDemand historyDemand)
        {
            if (historyDemand.StartDate > historyDemand.EndDate)
            {
                throw new BankException.InvalidDateRangeException();
            }
        }
        #endregion

        #region Account
        public async Task<AccountExistsResult> AccountExists(long idAccount)
        {
            var result = new AccountExistsResult();
            try
            {
                await _bankRepository.CheckAccount(idAccount);
                result.Result = AccountExistsResult.AccountStatus.Ok;
            }
            catch (BankException.InvalidAccountException)
            {
                result.Result = AccountExistsResult.AccountStatus.Invalid;
                result.Message = BankMessages.UnknownAccount;
            }
            return result;
        }
        #endregion

        #region Private Methods
        private async Task<TransactionResult> ExecuteTransactionAsync(Func<Task<TransactionResult>> transactionFunc)
        {
            try
            {
                return await transactionFunc();
            }
            catch (BankException.InvalidAccountException)
            {
                return CreateTransactionResult(TransactionResult.TransactionStatus.UnknownAccount, BankMessages.UnknownAccount);
            }
            catch (BankException.InsufficientFundsException)
            {
                return CreateTransactionResult(TransactionResult.TransactionStatus.InsufficientFunds, BankMessages.InsufficientFunds);
            }
            catch (BankException.TransactionErrorException)
            {
                return CreateTransactionResult(TransactionResult.TransactionStatus.Invalid, BankMessages.TransactionInfrastructureError);
            }
            catch (Exception)
            {
                return CreateTransactionResult(TransactionResult.TransactionStatus.Invalid, BankMessages.TransactionException);
            }
        }

        private TransactionResult CreateTransactionResult(TransactionResult.TransactionStatus status, string message)
        {
            return new TransactionResult { Result = status, Message = message };
        }

        private HistoryResult CreateHistoryResult(HistoryResult.HistoryStatus status, string message)
        {
            return new HistoryResult { Result = status, Message = message };
        }
        #endregion
    }
}
