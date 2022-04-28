using Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Application.Test
{
    [Collection("TestCollection1")]
    public class DepositTest : BankTest
    {
        [Fact]
        public async Task DepositOK()
        {
            var bank = this.GetBank();

            var demand = new DepositDemand() { IdAccount = 1, Amount = new DepositAmount(100), TransactionDate = System.DateTime.Now };
            var result = await bank.AddDeposit(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Ok);
        }

        [Fact]
        public void DepositKOWrongAmount()
        {
            Assert.Throws<Domain.BankException.InvalidAmountException>(
                () => new DepositDemand() { IdAccount = 1, Amount = new DepositAmount(-100), TransactionDate = System.DateTime.Now }
            );
        }
    }
}
