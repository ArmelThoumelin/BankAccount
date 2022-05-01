namespace Domain.Models
{
    public class HistoryAmount : IAmount
    {
        public decimal Value { get; set; }

        public HistoryAmount(decimal Amount)
        {
            this.Value = Amount;
        }
    }
}
