using System.Collections.Generic;

namespace Infrastructure.InMemoryDB.DBModels
{
    public class Account
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
