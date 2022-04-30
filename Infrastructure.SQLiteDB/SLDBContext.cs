using Infrastructure.SQLiteDB.DBModels;
using System.Data.Entity;

namespace Infrastructure.SQLiteDB
{
    public class SLDBContext : DbContext
    {
        public DbSet<Account> Users { get; set; }
        public SLDBContext() : base("SQLiteDatabase")
        {
            Database.SetInitializer(new NullDatabaseInitializer<SLDBContext>()); 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new SLDbContextInitializer());
        }
    }
}
