using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBank
    {
        Task<TransactionResult> AddDeposit(DepositDemand depositDemand);
    }

}
