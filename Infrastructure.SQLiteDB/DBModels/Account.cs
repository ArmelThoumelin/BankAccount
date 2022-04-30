using System.Collections.Generic;

namespace Infrastructure.SQLiteDB.DBModels
{
    public class Account
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
