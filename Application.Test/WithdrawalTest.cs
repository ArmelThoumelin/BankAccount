using Domain;
using Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class WithdrawalTest
    {
        private readonly Mock<IBankRepository> _mockBankRepository;
        private readonly Bank _bank;

        public WithdrawalTest()
        {
            _mockBankRepository = new Mock<IBankRepository>();
            _bank = new Bank(_mockBankRepository.Object);
        }

        [Fact]
        public async Task AddWithdrawal_ShouldReturnInsufficientFundsResult_WhenInsufficientFundsExceptionIsThrown()
        {
            // Arrange
            var withdrawalAmount = new WithdrawalAmount(100);
            var withdrawalDemand = new WithdrawalDemand { Amount = withdrawalAmount, IdAccount = 1 };
            _mockBankRepository.Setup(repo => repo.GetBalance(1)).ReturnsAsync(50);
            _mockBankRepository.Setup(repo => repo.AddTransaction(withdrawalDemand)).ThrowsAsync(new Domain.BankException.InsufficientFundsException());

            // Act
            var result = await _bank.AddWithdrawal(withdrawalDemand);

            // Assert
            Assert.Equal(TransactionResult.TransactionStatus.InsufficientFunds, result.Result);
            Assert.Equal(BankMessages.InsufficientFunds, result.Message);
        }
    }
}