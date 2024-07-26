using Domain;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class BalanceTest
    {
        private readonly Mock<IBankRepository> _mockBankRepository;
        private readonly Bank _bank;

        public BalanceTest()
        {
            _mockBankRepository = new Mock<IBankRepository>();
            _bank = new Bank(_mockBankRepository.Object);
        }

        [Fact]
        public async Task GetBalance_ShouldReturnCorrectBalance()
        {
            // Arrange
            var accountId = 1;
            var expectedBalance = 200;
            _mockBankRepository.Setup(repo => repo.GetBalance(accountId)).ReturnsAsync(expectedBalance);

            // Act
            var balance = await _bank.GetBalance(accountId);

            // Assert
            Assert.Equal(expectedBalance, balance);
        }
    }
}