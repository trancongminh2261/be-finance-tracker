using FinanceTracker.Entities.Base;
using FinanceTracker.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Repository
{
    public  class UnitOfWork : IUnitOfWork
    {
        protected IAppDbContext context;

        public UnitOfWork(IAppDbContext context)
        {
            this.context = context;
            this.context = context;
            if (this.context != null)
            {
                //this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                this.context.ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }

        public idomainRepository<T> Repository<T>() where T : BaseEntity
        {
            return new DomainRepository<T>(context);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
