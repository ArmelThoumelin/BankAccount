using Domain;
using Domain.Models;
using Infrastructure.InMemoryDB;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Application.ConsoleApp
{
    public class BankOperation
    {
        private const string Exit = "EXIT";
        private const string Withdrawal = "WITHDRAWAL";
        private const string Deposit = "DEPOSIT";
        private const string Consult = "CONSULT";
        private const string ShortenedWithdrawal = "W";
        private const string ShortenedDeposit = "D";
        private const string ShortenedConsult = "C";
        private const string DateFormatString = "dd/MM/yyyy";
        private Bank _Bank { get; set; }
        private long _IdAccount { get; set; }
        private Step _step { get; set; }

        enum Step
        {
            Exit,
            Login,
            OperationChoosing,
            Deposit,
            Withdrawal,
            History
        }

        public async Task Start()
        {
            var repository = new IMBankRepository();
            _Bank = new Bank(repository);
            _step = Step.Login;

            Console.WriteLine(ConsoleResource.Welcome);
            _step = await WelcomeAndAccountInput();
            if (_step != Step.Exit)
            {
                Console.Clear();
                await ChooseOperation();
            }

            Console.WriteLine(ConsoleResource.GoodBye);
            Console.ReadKey();
        }

        private async Task<Step> WelcomeAndAccountInput()
        {
            Step result = Step.Login;

            while (result == Step.Login)
            {
                Console.WriteLine(ConsoleResource.TypeAccountNumber);
                string entry = Console.ReadLine();
                if (IsCommand(entry, Exit))
                {
                    result = Step.Exit;
                }
                else
                {
                    if (long.TryParse(entry, out long idAccount))
                    {
                        var checkResult = await _Bank.AccountExists(idAccount);
                        if (checkResult.Result == AccountExistsResult.AccountStatus.Ok)
                        {
                            result = Step.OperationChoosing;
                            _IdAccount = idAccount;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(checkResult.Message);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(ConsoleResource.AccountNumberNumericOnly);
                    }
                }
            }
            return result;
        }

        private async Task ChooseOperation()
        {
            Step result = Step.OperationChoosing;
            while (result != Step.Exit)
            {
                Console.WriteLine(ConsoleResource.SelectOperation);
                Console.WriteLine(ConsoleResource.MakeDeposit);
                Console.WriteLine(ConsoleResource.MakeWithdrawal);
                Console.WriteLine(ConsoleResource.Consult);
                Console.WriteLine(ConsoleResource.OrTypeExit);

                string entry = Console.ReadLine();
                if (IsCommand(entry, Exit))
                {
                    result = Step.Exit;
                }
                else
                {
                    switch (entry.ToUpperInvariant())
                    {
                        case Deposit:
                        case ShortenedDeposit:
                            await MakeDeposit();
                            result = Step.Exit;
                            break;
                        case Withdrawal:
                        case ShortenedWithdrawal:
                            await MakeWithdrawal();
                            result = Step.Exit;
                            break;
                        case Consult:
                        case ShortenedConsult:
                            await SeeHistory();
                            result = Step.Exit;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine(ConsoleResource.UnrecognizedCommand, entry);
                            break;
                    }
                }
            }
        }

        #region History
        private async Task SeeHistory()
        {
            DateTime? startDate = GetDateEntry(ConsoleResource.Start);
            if (startDate.HasValue)
            {
                DateTime? endDate = GetDateEntry(ConsoleResource.End);
                if (endDate.HasValue)
                {
                    var demand = new HistoryDemand() { IdAccount = _IdAccount, StartDate = startDate.Value, EndDate = endDate.Value };
                    var transactions = await _Bank.GetTransactions(demand);

                    if (transactions.Result == HistoryResult.HistoryStatus.Ok)
                    {
                        var balance = await _Bank.GetBalance(_IdAccount);
                        DisplayHistory(transactions, balance);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(transactions.Message);
                    }
                    await ChooseOperation();
                }
            }
        }

        private DateTime? GetDateEntry(string dateKind)
        {
            DateTime? result = null;
            string entry = string.Empty;
            Console.Clear();
            while (!result.HasValue && !IsCommand(entry, Exit))
            {
                Console.WriteLine(ConsoleResource.EnterDate, dateKind);
                entry = Console.ReadLine();

                if (!IsCommand(entry, Exit))
                {
                    if (DateTime.TryParseExact(entry, DateFormatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime entryDateTime))
                    {
                        result = entryDateTime;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(ConsoleResource.IncorrectDateEntry);
                    }
                }
            }
            return result;
        }

        private void DisplayHistory(HistoryResult historyResult, decimal balance)
        {
            Console.Clear();
            Console.WriteLine(ConsoleResource.YourTransactions);
            foreach (var transaction in historyResult.Transactions)
            {
                Console.WriteLine("{0} - {1} : {2}", transaction.IdTransaction, transaction.TransactionDate, transaction.Amount.Value);
            }
            Console.WriteLine(ConsoleResource.BalanceTotal, balance);
            Console.WriteLine(ConsoleResource.PressAnyKey);
            Console.ReadKey();
            Console.Clear();

        }
        #endregion

        private async Task MakeDeposit()
        {
            decimal? amount = GetAmount();
            if (amount.HasValue)
            {
                var demand = new DepositDemand() { IdAccount = _IdAccount, Amount = new DepositAmount(amount.Value), TransactionDate = DateTime.Now };
                var result = await _Bank.AddDeposit(demand);
                Console.Clear();
                if (result.Result == TransactionResult.TransactionStatus.Ok)
                {
                    Console.WriteLine(ConsoleResource.DepositRegistered, amount.Value);
                }
                else
                {
                    Console.WriteLine(result.Message);
                }
                await ChooseOperation();
            }
        }

        private async Task MakeWithdrawal()
        {
            decimal? amount = GetAmount();
            if (amount.HasValue)
            {
                var demand = new WithdrawalDemand() { IdAccount = _IdAccount, Amount = new WithdrawalAmount(amount.Value), TransactionDate = DateTime.Now };
                var result = await _Bank.AddWithdrawal(demand);
                Console.Clear();
                if (result.Result == TransactionResult.TransactionStatus.Ok)
                {
                    Console.WriteLine(ConsoleResource.WithdrawalRegistered, amount.Value);
                }
                else
                {
                    Console.WriteLine(result.Message);
                }
                await ChooseOperation();
            }
        }

        private decimal? GetAmount()
        {
            decimal? result = null;
            string entry = string.Empty;
            Console.Clear();
            while (!result.HasValue && !IsCommand(entry, Exit))
            {
                Console.WriteLine(ConsoleResource.EnterAmount);
                entry = Console.ReadLine();

                if (!IsCommand(entry, Exit))
                {
                    if (decimal.TryParse(entry, out decimal entryDecimal))
                    {
                        if (entryDecimal <= decimal.Zero)
                        {
                            Console.Clear();
                            Console.WriteLine(ConsoleResource.PositiveAmountOnly);
                        }
                        else
                        {
                            result = entryDecimal;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(ConsoleResource.PositiveCommaSeparatedAmountOnly);
                    }
                }
            }
            return result;
        }

        private bool IsCommand(string entry, string command)
        {
            return !string.IsNullOrEmpty(entry) &&
                (entry.Equals(command, StringComparison.InvariantCultureIgnoreCase) ||
                entry.ToString().Equals(command[0].ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}