using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Extensions;
using MonaDotNetTemplate.Repository.Interfaces;
using MonaDotNetTemplate.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Repository
{
    public class DomainRepository<T> : IDomainRepository<T> where T : BaseEntity
    {
        protected readonly IAppDbContext Context;

        public DomainRepository(IAppDbContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(T entity)
        {
            var user = LoginContext.Instance?.CurrentUser;
            if (user != null)
            {
                entity.Created = Timestamp.Now;
                entity.CreatedBy = user.Id;
            }
            await Context.Set<T>().AddAsync(entity);
        }

        public async Task CreateAsync(IList<T> entities)
        {
            var user = LoginContext.Instance.CurrentUser;
            if (user != null)
            {
                foreach (var entity in entities)
                {
                    entity.CreatedBy = user.Id;
                    entity.Created = Timestamp.Now;
                }
            }
            await Context.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void Delete(IList<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }

        public void Detach(T entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        public SPM ExcuteQuery<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : class
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);
                return MappingDataTable.ConvertToList<SPM>(dataTable).FirstOrDefault();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public async Task<SPM> ExcuteQueryAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : class
        {

            DataTable dataTable = new DataTable();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);
                return MappingDataTable.ConvertToList<SPM>(dataTable).FirstOrDefault();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public async Task<List<SPM>> ExcuteQueryListAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : class
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);
                return MappingDataTable.ConvertToList<SPM>(dataTable);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public async Task<BasePagination<SPM>> ExcuteQueryPagingAsync<SPM>(string commandText, SqlParameter[] sqlParameters) where SPM : BasePaginationItem
        {

            BasePagination<SPM> pagedList = new BasePagination<SPM>();
            DataTable dataTable = new DataTable();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);
                pagedList.Items = MappingDataTable.ConvertToList<SPM>(dataTable);
                if (pagedList.Items != null && pagedList.Items.Any())
                    pagedList.TotalItem = pagedList.Items.FirstOrDefault().TotalItem;
                return pagedList;
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public async Task<BasePagination<SPM>> ExcuteQueryFromFilePagingAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : BasePaginationItem
        {
            BasePagination<SPM> pagedList = new BasePagination<SPM>();
            string sqlScript = File.ReadAllText(filePath);
            pagedList.Items = await Context.Database.SqlQueryRaw<SPM>(sqlScript, sqlParameters).ToListAsync();
            if (pagedList.Items != null && pagedList.Items.Any())
                pagedList.TotalItem = pagedList.Items.FirstOrDefault().TotalItem;
            return pagedList;
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] sqlParameters)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)Context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public IQueryable<T> GetQueryable()
        {
            return Context.Set<T>();
        }

        public void Update(T entity)
        {
            var user = LoginContext.Instance.CurrentUser;
            if (user != null)
            {
                entity.UpdatedBy = user.Id;
                entity.Updated = Timestamp.Now;
            }
            Context.Set<T>().Update(entity);
        }

        public void UpdateAndCheckField(T entity)
        {
            T oldEntity = Context.Set<T>().AsNoTracking().SingleOrDefault(x => x.Id == entity.Id);
            var typeOfEntity = typeof(T);
            foreach (var property in typeOfEntity.GetProperties())
            {
                if (property.GetValue(entity) != null)
                    continue;
                property.SetValue(entity, property.GetValue(oldEntity));
            }
            var user = LoginContext.Instance.CurrentUser;
            if (user != null)
            {
                entity.UpdatedBy = user.Id;
                entity.Updated = Timestamp.Now;
            }
            Context.Set<T>().Update(entity);
        }

        public int ExecuteNonQueryFromFile(string filePath, SqlParameter[] sqlParameters)
        {
            string sqlScript = File.ReadAllText(filePath);
            return Context.Database.ExecuteSqlRaw(sqlScript, sqlParameters);
        }

        public int ExecuteNonQueryFromFile(string filePath)
        {
            string sqlScript = File.ReadAllText(filePath);
            return Context.Database.ExecuteSqlRaw(sqlScript);
        }

        public SPM ExcuteQueryFromFile<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class
        {
            string sqlScript = File.ReadAllText(filePath);
            return Context.Database.SqlQueryRaw<SPM>(sqlScript, sqlParameters).SingleOrDefault();
        }

        public async Task<List<SPM>> ExcuteQueryFromFileListAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class
        {
            string sqlScript = File.ReadAllText(filePath);
            return await Context.Database.SqlQueryRaw<SPM>(sqlScript, sqlParameters).ToListAsync();
        }

        public async Task<SPM> ExcuteQueryFromFileAsync<SPM>(string filePath, SqlParameter[] sqlParameters) where SPM : class
        {
            string sqlScript = File.ReadAllText(filePath);
            return await Context.Database.SqlQueryRaw<SPM>(sqlScript, sqlParameters).SingleOrDefaultAsync();
        }

        public virtual void SetEntityState(T item, EntityState entityState)
        {
            Context.Entry(item).State = entityState;
        }

        public virtual bool UpdateFieldsSave(T entity, params Expression<Func<T, object>>[] includeProperties)
        {
            entity.Updated = Timestamp.Now;
            if (entity.UpdatedBy != null)
            {
                var User = LoginContext.Instance.CurrentUser;
                if (User != null)
                {
                    entity.UpdatedBy = User.Id;
                }
            }
            var dbEntry = Context.Entry(entity);

            foreach (var includeProperty in includeProperties)
            {
                dbEntry.Property(includeProperty).IsModified = true;
            }
            Context.SaveChanges();
            return true;
        }
    }
}
