using Domain.Models;

namespace Domain
{
    public interface IBank
    {
        TransactionResult AddDeposit(DepositDemand depositDemand);
    }

}
