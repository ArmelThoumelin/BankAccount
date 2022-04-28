using Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Application.Test
{
    [Collection("TestCollection1")]
    public class WithdrawalTest : BankTest
    {
        [Fact]
        public async Task WithDrawalOK()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = _AccountOk, Amount = new WithdrawalAmount(100), TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.Ok);
        }

        [Fact]
        public void WithDrawalKOWrongAmount()
        {
            Assert.Throws<Domain.BankException.InvalidAmountException>(
                () => new WithdrawalDemand() { IdAccount = _AccountOk, Amount = new WithdrawalAmount(-100), TransactionDate = System.DateTime.Now }
            );
        }

        [Fact]
        public async Task WithDrawalKOWrongAccount()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = _AccountKo, Amount = new WithdrawalAmount(100), TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.UnknownAccount);
        }

        [Fact]
        public async Task WithDrawalKOAmountExceedingSavings()
        {
            var bank = this.GetBank();

            var demand = new WithdrawalDemand() { IdAccount = _AccountOk, Amount = new WithdrawalAmount(decimal.MaxValue), TransactionDate = System.DateTime.Now };
            var result = await bank.AddWithdrawal(demand);

            Assert.True(result.Result == TransactionResult.TransactionStatus.InsufficientFunds);
        }
    }
}
