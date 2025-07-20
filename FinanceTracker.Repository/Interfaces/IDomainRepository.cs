using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Repository.Interfaces
{
    public interface idomainRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQueryable();
        Task CreateAsync(T entity);
        Task CreateAsync(IList<T> entities);
        void Update(T entity);
        void UpdateAndCheckField(T entity);
        void Delete(T entity);
        void Delete(IList<T> entities);
        void Detach(T entity);
        int ExecuteNonQuery(string commandText, SqlParameter[] sqlParameters);
        int ExecuteNonQuery(string commandText);
        int ExecuteNonQueryFromFile(string filePath, SqlParameter[] sqlParameters);
        int ExecuteNonQueryFromFile(string filePath);
        SPM ExcuteQuery<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM: class;
        SPM ExcuteQueryFromFile<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class;
        Task<BasePagination<SPM>> ExcuteQueryPagingAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : BasePaginationItem;
        Task<List<SPM>> ExcuteQueryListAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : class;
        Task<SPM> ExcuteQueryAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM: class;
        Task<List<SPM>> ExcuteQueryFromFileListAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class;
        Task<SPM> ExcuteQueryFromFileAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class;
        Task<BasePagination<SPM>> ExcuteQueryFromFilePagingAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : BasePaginationItem;
        void SetEntityState(T item, EntityState entityState);
        bool UpdateFieldsSave(T entity, params Expression<Func<T, object>>[] includeProperties);
    }
}
