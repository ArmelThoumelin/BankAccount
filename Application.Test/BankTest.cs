using Domain;
using Infrastructure.InMemoryDB;

namespace Application.Test
{
    public abstract class BankTest
    {
        IBank _bank;

        protected IBank GetBank()
        {
            var bankRepository = new IMBankRepository();
            if (_bank == null)
            {
                _bank = new Bank(bankRepository);
            }
            return _bank;
        }
    }
}
