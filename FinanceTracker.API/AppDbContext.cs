using Microsoft.EntityFrameworkCore;
using FinanceTracker.Entities.Entities;
using FinanceTracker.Repository.Interfaces;

namespace FinanceTracker.API
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(x => x.ToTable("Account"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
