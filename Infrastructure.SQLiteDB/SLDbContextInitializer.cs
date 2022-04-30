using Infrastructure.SQLiteDB.DBModels;
using System.Collections.Generic;

namespace Infrastructure.SQLiteDB
{
    public class SLDbContextInitializer : System.Data.Entity.CreateDatabaseIfNotExists<SLDBContext>
    {
        protected override void Seed(SLDBContext context)
        {
            var account1 = new Account() { Id = 1, Owner = "Mr Dupont", Transactions = new List<Transaction>() };
            context.Set<Account>().Add(account1);
            var account2 = new Account() { Id = 2, Owner = "Mme Durand", Transactions = new List<Transaction>() };
            context.Set<Account>().Add(account2);

            account1.Transactions.Add(new Transaction() { Id = 1, IdAccount = 1, Amount = 2000, TransactionDate = System.DateTime.Now });
            account1.Transactions.Add(new Transaction() { Id = 2, IdAccount = 1, Amount = 200, TransactionDate = System.DateTime.Now });
            account1.Transactions.Add(new Transaction() { Id = 3, IdAccount = 1, Amount = 20, TransactionDate = System.DateTime.Now });
            account2.Transactions.Add(new Transaction() { Id = 4, IdAccount = 2, Amount = 4000, TransactionDate = System.DateTime.Now });
            account2.Transactions.Add(new Transaction() { Id = 5, IdAccount = 2, Amount = 400, TransactionDate = System.DateTime.Now });
            account2.Transactions.Add(new Transaction() { Id = 6, IdAccount = 2, Amount = 40, TransactionDate = System.DateTime.Now });
            context.SaveChanges(); 
            
            base.Seed(context);
        }
    }
}
