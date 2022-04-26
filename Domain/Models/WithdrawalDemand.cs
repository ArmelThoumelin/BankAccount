namespace Domain.Models
{
    public class WithdrawalDemand : TransactionDemand
    {
        public decimal WithdrawalAmount { get => this.Amount; set => this.Amount = value * -1; }
    }
}
