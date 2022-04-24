using System;

namespace Domain.Models
{
    public class TransactionDemand
    {
        public long IdAccount { get; set; }
        public decimal Amount { get; protected set; }
        public DateTime TransactionDate { get; set; }
    }
}
