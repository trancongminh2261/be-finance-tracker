using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IDomainRepository<T> Repository<T>() where T : BaseEntity;
        Task SaveAsync();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
    }
}
