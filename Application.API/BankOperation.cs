using Domain;

namespace Application.API
{
    public class BankOperation
    {
        private IBank _Bank { get; set; }

        public BankOperation(IBank bank)
        {
            _Bank = bank;
        }

    }
}
