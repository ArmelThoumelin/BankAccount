using Infrastructure.InMemoryDB.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InMemoryDB
{
    public class IMDBContext : DbContext
    {
        public IMDBContext(DbContextOptions<IMDBContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Users { get; set; }

        public DbSet<Transaction> Posts { get; set; }
    }
}