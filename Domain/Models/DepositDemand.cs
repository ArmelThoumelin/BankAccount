namespace Domain.Models
{
    public class DepositDemand : TransactionDemand
    {
        public decimal DepositAmount { get => this.Amount; set => this.Amount = value; }
    }
}
