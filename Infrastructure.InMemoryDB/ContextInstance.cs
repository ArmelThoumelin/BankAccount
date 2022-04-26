using Infrastructure.InMemoryDB.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InMemoryDB
{
    public static class ContextInstance
    {
        private static IMDBContext iMDBContext { get; set; }

        public static IMDBContext GetInstance()
        {
            if (iMDBContext == null)
            {
                iMDBContext = PrepareInstance();
            }

            return iMDBContext;
        }

        private static IMDBContext PrepareInstance()
        {
            CreateContext();

            Fill();

            return iMDBContext;
        }

        private static void CreateContext()
        {
            var builder = new DbContextOptionsBuilder<IMDBContext>();
            builder.UseInMemoryDatabase(databaseName: "BankInMemoryDB");


            var dbContextOptions = builder.Options;
            iMDBContext = new IMDBContext(dbContextOptions);

            iMDBContext.Database.EnsureDeleted();
            iMDBContext.Database.EnsureCreated();
        }

        private static void Fill()
        {
            iMDBContext.Add<Account>(new Account() { Id = 1, Owner = "Mr Dupont" });
            iMDBContext.Add<Transaction>(new Transaction() { Id = 1, IdAccount = 1, Amount = 2000, TransactionDate = System.DateTime.Now });
            iMDBContext.SaveChanges();
        }
    }
}
