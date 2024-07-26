using Domain;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class HistoryTests
    {
        private readonly Mock<IBankRepository> _mockBankRepository;
        private readonly Bank _bank;

        public HistoryTests()
        {
            _mockBankRepository = new Mock<IBankRepository>();
            _bank = new Bank(_mockBankRepository.Object);
        }

        [Fact]
        public async Task GetTransactions_ShouldReturnOkResult_WhenDatesAreValid()
        {
            // Arrange
            var historyDemand = new HistoryDemand { StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now };
            var expectedResult = new HistoryResult { Result = HistoryResult.HistoryStatus.Ok };
            _mockBankRepository.Setup(repo => repo.GetTransactions(historyDemand)).ReturnsAsync(expectedResult.Transactions);

            // Act
            var result = await _bank.GetTransactions(historyDemand);

            // Assert
            Assert.Equal(HistoryResult.HistoryStatus.Ok, result.Result);
        }

        [Fact]
        public async Task GetTransactions_ShouldReturnInvalidDateRangeResult_WhenInvalidDateRangeExceptionIsThrown()
        {
            // Arrange
            var historyDemand = new HistoryDemand { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-10) };
            _mockBankRepository.Setup(repo => repo.GetTransactions(historyDemand)).ThrowsAsync(new Domain.BankException.InvalidDateRangeException());

            // Act
            var result = await _bank.GetTransactions(historyDemand);

            // Assert
            Assert.Equal(HistoryResult.HistoryStatus.InvalidDateRange, result.Result);
            Assert.Equal(BankMessages.HistoryDateRangeError, result.Message);
        }
    }
}