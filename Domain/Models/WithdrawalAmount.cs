using System;

namespace Domain.Models
{
    public class WithdrawalAmount : IAmount
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
                _Amount = value * -1;
            }
        }

        public WithdrawalAmount(decimal Amount)
        {
            this.Value = Amount;
        }

        public static bool operator < (decimal left, WithdrawalAmount right) => left < Math.Abs(right.Value);
        public static bool operator > (decimal left, WithdrawalAmount right) => left > Math.Abs(right.Value);
    }
}
