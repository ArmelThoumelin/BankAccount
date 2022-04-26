namespace Domain.Models
{
    public class DepositAmount : IAmount
    {
        private decimal _Amount;

        public decimal Value
        {
            get => _Amount;
            set
            {
                if (value <= decimal.Zero)
                {
                    throw new BankException.InvalidAmountException();
                }
                _Amount = value;
            }
        }

        public DepositAmount(decimal Amount)
        {
            this.Value = Amount;
        }
    }
}
