using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Interfaces.Base
{
    public interface IBaseService<Entity, GetPageR>
        where Entity : BaseEntity where GetPageR : BaseGetPaginationRequest
    {
        Task<BasePagination<SPM>> GetPagedListData<SPM>(GetPageR baseSearch) where SPM : BasePaginationItem;
        Task<IList<Entity>> GetAsync(Expression<Func<Entity, bool>> expression);
        Task<Model> GetByIdAsync<Model>(Guid id) where Model : BaseModel;
        Task<Entity> GetSingleAsync(Expression<Func<Entity, bool>> expression);
        Task<bool> UpdateAsync(Entity item);
        Task<bool> UpdateAsync(IList<Entity> items);
        Task<bool> CreateAsync(Entity item);
        Task<bool> CreateAsync(IList<Entity> items);
        Task<bool> UpdateFieldAsync(IList<Entity> items, params Expression<Func<Entity, object>>[] includeProperties);
        Task<bool> UpdateFieldAsync(Entity item, params Expression<Func<Entity, object>>[] includeProperties);
        Task CheckAlreadyAsync( bool isNull);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveAsync(Entity item);
        Task<bool> SaveAsync(IList<Entity> items);
        SqlParameter[] GetSqlParameters(GetPageR baseSearch);
        string GetStoreProcedureName { get; set; }
        string GetSQLFilePath { get; set; }
        Expression<Func<Entity, bool>> CheckAlreadyExpression { get; set; }
    }
}
