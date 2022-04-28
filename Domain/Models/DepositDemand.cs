namespace Domain.Models
{
    public class DepositDemand : TransactionDemand<DepositAmount>
    {
        public override DepositAmount Amount { get; set; }
    }
}
