namespace Domain.Models
{
    public class WithdrawalDemand : TransactionDemand<WithdrawalAmount>
    {
        public override WithdrawalAmount Amount { get; set; }
    }
}
