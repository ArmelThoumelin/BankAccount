namespace Domain.Models
{
    public class HistoryAmount : IAmount
    {
        private decimal _Amount;

        public decimal Value { get; set; }

        public HistoryAmount(decimal Amount)
        {
            this.Value = Amount;
        }
    }
}
