using Domain.Models;
using System.Threading.Tasks;
using Xunit;


namespace Application.Test
{
    public class WithdrawalTest : BankTest
    {
        [Fact]
        public async Task WithDrawalOK()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = 1, WithdrawalAmount = 100, TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Ok);
        }

        [Fact]
        public async Task DepositKOWrongAmount()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = 1, WithdrawalAmount = -100, TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Unauthorized);
        }

        [Fact]
        public async Task WithDrawalKOWrongAccount()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = long.MaxValue, WithdrawalAmount = 100, TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Invalid);
        }

        [Fact]
        public async Task WithDrawalKOAmountExceedingSavings()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = 1, WithdrawalAmount = decimal.MaxValue, TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Invalid);
        }
    }
}
