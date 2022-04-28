using System;

namespace Domain.Models
{
    public class Transaction
    {
        public long IdTransaction { get; set; }
        public IAmount Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
