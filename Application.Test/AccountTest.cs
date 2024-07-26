using Domain;
using Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class AccountTest
    {
        private readonly Mock<IBankRepository> _mockBankRepository;
        private readonly Bank _bank;

        public AccountTest()
        {
            _mockBankRepository = new Mock<IBankRepository>();
            _bank = new Bank(_mockBankRepository.Object);
        }

        [Fact]
        public async Task AccountExists_ShouldReturnOkResult_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            _mockBankRepository.Setup(repo => repo.CheckAccount(accountId)).Returns(Task.CompletedTask);

            // Act
            var result = await _bank.AccountExists(accountId);

            // Assert
            Assert.Equal(AccountExistsResult.AccountStatus.Ok, result.Result);
        }

        [Fact]
        public async Task AccountExists_ShouldReturnInvalidResult_WhenInvalidAccountExceptionIsThrown()
        {
            // Arrange
            var accountId = 1;
            _mockBankRepository.Setup(repo => repo.CheckAccount(accountId)).ThrowsAsync(new Domain.BankException.InvalidAccountException());

            // Act
            var result = await _bank.AccountExists(accountId);

            // Assert
            Assert.Equal(AccountExistsResult.AccountStatus.Invalid, result.Result);
            Assert.Equal(BankMessages.UnknownAccount, result.Message);
        }
    }
}