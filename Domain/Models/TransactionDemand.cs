using System;

namespace Domain.Models
{
    public abstract class TransactionDemand<T> where T : IAmount
    {
        public long IdAccount { get; set; }
        public abstract T Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
