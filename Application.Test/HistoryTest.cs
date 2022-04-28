using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.Test
{
    [Collection("TestCollection1")]
    public class HistoryTest : BankTest
    {
        private Domain.IBank bank { get=> this.GetBank(); }

        [Fact]
        public async Task BalanceEqualTransactionsSumOK()
        {
            // Arrange
            var demand = new HistoryDemand() { IdAccount = 1, StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue };

            // Act
            decimal balance = await bank.GetBalance(1);
            var Transactions = await bank.GetTransactions(demand);
            decimal TransactionsSum = Transactions.Transactions.Sum(t => t.Amount.Value);

            // Assert
            Assert.True(balance == TransactionsSum);
        }

        [Fact]
        public async Task DateFilterRetieveExpectedTransactionOK()
        {
            // Arrange
            var amounts = new List<decimal>() { 123, 456, 789 };
            var dates = new List<DateTime>() { new DateTime(1955, 11, 5), new DateTime(1955, 11, 12), new DateTime(1885, 1, 1)};
            int firstTransactionsIndex = 0;
            int lastTransactionsIndex = 1;

            var demand = new HistoryDemand() { IdAccount = 1, StartDate = dates[firstTransactionsIndex], EndDate = dates[lastTransactionsIndex] };
            var expectedSum = amounts[firstTransactionsIndex] + amounts[lastTransactionsIndex];

            // Act
            for (int i = 0; i < amounts.Count; i++)
            {
                await SendDeposit(1, amounts[i], dates[i]);
            }
            var Transactions = await bank.GetTransactions(demand);
            decimal TransactionsSum = Transactions.Transactions.Sum(t => t.Amount.Value);

            // Assert
            Assert.True(Transactions.Transactions.Count == 2);
            Assert.True(TransactionsSum == expectedSum);
            Assert.True(Transactions.Transactions.Min(t => t.TransactionDate) >= dates[firstTransactionsIndex]);
            Assert.True(Transactions.Transactions.Max(t => t.TransactionDate) <= dates[lastTransactionsIndex]);
        }

        private async Task SendDeposit(long IdAccount, decimal Amount, DateTime TransactionDate)
        {
            var demand = new DepositDemand() { IdAccount = IdAccount, Amount = new DepositAmount(Amount), TransactionDate = TransactionDate };
            await bank.AddDeposit(demand);

        }

        [Fact]
        public async Task StartDateAfterEndDateKO()
        {
            // Arrange
            var demand = new HistoryDemand() { IdAccount = 1, StartDate = DateTime.MaxValue, EndDate = DateTime.MinValue };

            // Act 
            var result = await bank.GetTransactions(demand);

            // Assert
            Assert.True(result.Result == HistoryResult.HistoryStatus.InvalidDateRange);
        }

        [Fact]
        public async Task HistoryKOWrongAccount()
        {
            // Arrange
            var demand = new HistoryDemand() { IdAccount = long.MaxValue, StartDate = DateTime.Now, EndDate = DateTime.Now };

            // Act 
            var result = await bank.GetTransactions(demand);

            // Assert
            Assert.True(result.Result == HistoryResult.HistoryStatus.UnknownAccount);
        }
    }
}
