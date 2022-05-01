using Infrastructure.InMemoryDB.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InMemoryDB
{
    public static class ContextInstance
    {
        private static IMDBContext _IMDBContext { get; set; }

        public static IMDBContext GetInstance()
        {
            if (_IMDBContext == null)
            {
                _IMDBContext = PrepareInstance();
            }

            return _IMDBContext;
        }

        private static IMDBContext PrepareInstance()
        {
            CreateContext();

            Fill();

            return _IMDBContext;
        }

        private static void CreateContext()
        {
            var builder = new DbContextOptionsBuilder<IMDBContext>();
            builder.UseInMemoryDatabase(databaseName: "BankInMemoryDB");


            var dbContextOptions = builder.Options;
            _IMDBContext = new IMDBContext(dbContextOptions);

            _IMDBContext.Database.EnsureDeleted();
            _IMDBContext.Database.EnsureCreated();
        }

        private static void Fill()
        {
            _IMDBContext.Add<Account>(new Account() { Id = 1, Owner = "Mr Dupont" });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 1, IdAccount = 1, Amount = 2000, TransactionDate = System.DateTime.Now });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 2, IdAccount = 1, Amount = 20000, TransactionDate = System.DateTime.Now });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 3, IdAccount = 1, Amount = 200000, TransactionDate = System.DateTime.Now });
            _IMDBContext.Add<Account>(new Account() { Id = 2, Owner = "Mme Duchemin" });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 4, IdAccount = 2, Amount = 6000, TransactionDate = System.DateTime.Now });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 5, IdAccount = 2, Amount = 60000, TransactionDate = System.DateTime.Now });
            _IMDBContext.Add<Transaction>(new Transaction() { Id = 6, IdAccount = 2, Amount = 600000, TransactionDate = System.DateTime.Now });
            _IMDBContext.SaveChanges();
        }
    }
}
