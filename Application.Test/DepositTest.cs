using Domain;
using Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class DepositTests
    {
        private readonly Mock<IBankRepository> _mockBankRepository;
        private readonly Bank _bank;

        public DepositTests()
        {
            _mockBankRepository = new Mock<IBankRepository>();
            _bank = new Bank(_mockBankRepository.Object);
        }

        [Fact]
        public async Task AddDeposit_ShouldReturnSuccessResult_WhenTransactionIsSuccessful()
        {
            // Arrange
            var depositAmount = new DepositAmount(100);
            var depositDemand = new DepositDemand { Amount = depositAmount, IdAccount = 1 };
            var expectedResult = new TransactionResult { Result = TransactionResult.TransactionStatus.Ok };
            _mockBankRepository.Setup(repo => repo.AddTransaction(depositDemand)).ReturnsAsync(expectedResult);

            // Act
            var result = await _bank.AddDeposit(depositDemand);

            // Assert
            Assert.Equal(TransactionResult.TransactionStatus.Ok, result.Result);
        }

        [Fact]
        public async Task AddDeposit_ShouldReturnUnknownAccountResult_WhenInvalidAccountExceptionIsThrown()
        {
            // Arrange
            var depositAmount = new DepositAmount(100);
            var depositDemand = new DepositDemand { Amount = depositAmount, IdAccount = 1 };
            _mockBankRepository.Setup(repo => repo.AddTransaction(depositDemand)).ThrowsAsync(new Domain.BankException.InvalidAccountException());

            // Act
            var result = await _bank.AddDeposit(depositDemand);

            // Assert
            Assert.Equal(TransactionResult.TransactionStatus.UnknownAccount, result.Result);
            Assert.Equal(BankMessages.UnknownAccount, result.Message);
        }
    }
}