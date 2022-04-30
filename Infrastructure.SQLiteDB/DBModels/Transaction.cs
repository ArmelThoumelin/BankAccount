using System;

namespace Infrastructure.SQLiteDB.DBModels
{
    public class Transaction
    {
        public long Id { get; set; }
        public long IdAccount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public Account Account { get; set; }

    }
}
