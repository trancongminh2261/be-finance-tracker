using Microsoft.EntityFrameworkCore;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Repository.Interfaces;

namespace MonaDotNetTemplate.API
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(x => x.ToTable("Role"));
            modelBuilder.Entity<Account>(x => x.ToTable("Account"));
            modelBuilder.Entity<AccountInfo>(x => x.ToTable("AccountInfo"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
