using Mapster;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FinanceTracker.Entities.Base;
using FinanceTracker.Repository.Interfaces;
using FinanceTracker.Services.Interfaces.Base;
using FinanceTracker.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services.Services.Base
{
    public abstract class BaseService<Entity, GetPageR> : IBaseService<Entity, GetPageR>
         where Entity : BaseEntity where GetPageR : BaseGetPaginationRequest, new()
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IAppDbContext Context;

        public string GetStoreProcedureName { get; set; }
        public string GetSQLFilePath { get; set; }
        public Expression<Func<Entity, bool>> CheckAlreadyExpression { get; set; }


        protected BaseService(IUnitOfWork unitOfWork, IAppDbContext context)
        {
            this.unitOfWork = unitOfWork;
            Context = context;
        }

        public virtual async Task<bool> CreateAsync(Entity item)
        {
            return await CreateAsync(new List<Entity> { item });
        }

        public virtual async Task<bool> CreateAsync(IList<Entity> items)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        CheckAlreadyExpression = null;
                        await CheckAlreadyAsync(true);
                        await unitOfWork.Repository<Entity>().CreateAsync(item);
                    }
                    await unitOfWork.SaveAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch
                {
                    await dbContextTransaction.RollbackAsync();
                    throw new AppException(CoreContant.ResponseMessageType.BadRequest);
                }
            }
            return true;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await unitOfWork.Repository<Entity>().GetQueryable().SingleOrDefaultAsync(x => x.id == id);
            if (exists == null)
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
            exists.is_deleted = true;
            unitOfWork.Repository<Entity>().Update(exists);
            await unitOfWork.SaveAsync();
            return true;
        }
        // nguy hiểm
        public virtual async Task<IList<Entity>> GetAsync(Expression<Func<Entity, bool>> expression)
        {
            var result = await unitOfWork.Repository<Entity>()
                .GetQueryable()
                .Where(expression)
                .ToListAsync();
            if (result == null || !result.Any())
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
            return result;
        }

        public virtual async Task<Model> GetByidAsync<Model>(Guid id) where Model : BaseModel
        {
            var entity = await unitOfWork.Repository<Entity>().GetQueryable().Where(e => e.id == id && !e.is_deleted).AsNoTracking().SingleOrDefaultAsync();
            if (entity == null)
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
            return entity.Adapt<Model>();
        }

        public virtual async Task CheckAlreadyAsync(bool isNull)
        {
            if (CheckAlreadyExpression == null)
                return;
            var already = await unitOfWork.Repository<Entity>().GetQueryable().AsNoTracking().FirstOrDefaultAsync(CheckAlreadyExpression);
            if (isNull == (already == null))
                return;

            if (isNull)
                throw new AppException(CoreContant.ResponseMessageType.Duplicate);

            throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
        }

        public virtual async Task<BasePagination<SPM>> GetPagedListData<SPM>(GetPageR baseSearch) where SPM : BasePaginationItem
        {
            BasePagination<SPM> pagedList = new BasePagination<SPM>();
            int pageIndexTmp = baseSearch.PageIndex ?? 0;
            baseSearch.PageIndex = (baseSearch.PageIndex - 1) * baseSearch.PageSize;
            SqlParameter[] parameters = GetSqlParameters(baseSearch);
            pagedList = await unitOfWork.Repository<Entity>().ExcuteQueryFromFilePagingAsync<SPM>(GetSQLFilePath, parameters);
            if (!pagedList.Items.Any())
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
            pagedList.PageIndex = pageIndexTmp;
            pagedList.PageSize = baseSearch.PageSize ?? 0;
            return pagedList;
        }

        public async Task<Entity> GetSingleAsync(Expression<Func<Entity, bool>> expression)
        {
            var result = await unitOfWork.Repository<Entity>().GetQueryable().FirstOrDefaultAsync(expression);
            if (result == null)
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [typeof(Entity).Name]);
            return result;
        }

        public async Task<bool> SaveAsync(Entity item)
        {
            return await SaveAsync(new List<Entity> { item });
        }

        public async Task<bool> SaveAsync(IList<Entity> items)
        {
            foreach (var item in items)
            {
                var exists = await unitOfWork.Repository<Entity>().GetQueryable().AsNoTracking()
                .Where(e => e.id == item.id && !e.is_deleted).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (item.is_deleted)
                    {
                        await DeleteAsync(item.id);
                    }
                    else
                    {
                        exists = item.Adapt<Entity>();
                        unitOfWork.Repository<Entity>().SetEntityState(exists, EntityState.Modified);
                    }
                }
                else
                {
                    await unitOfWork.Repository<Entity>().CreateAsync(item);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Entity item)
        {
            return await UpdateAsync(new List<Entity> { item });
        }

        public async Task<bool> UpdateAsync(IList<Entity> items)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        CheckAlreadyExpression = x => x.id == item.id;
                        await CheckAlreadyAsync(false);
                        unitOfWork.Repository<Entity>().UpdateAndCheckField(item);
                    }
                    await unitOfWork.SaveAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch
                {
                    await dbContextTransaction.RollbackAsync();
                    throw new AppException(CoreContant.ResponseMessageType.BadRequest);
                }
            }
            return true;
        }

        public async Task<bool> UpdateFieldAsync(IList<Entity> items, params Expression<Func<Entity, object>>[] includeProperties)
        {
            foreach (var item in items)
            {
                var exists = await unitOfWork.Repository<Entity>().GetQueryable()
                 .AsNoTracking()
                 .Where(e => e.id == item.id && !e.is_deleted)
                 .FirstOrDefaultAsync();
                if (exists != null)
                {
                    exists = item.Adapt<Entity>();
                    unitOfWork.Repository<Entity>().UpdateFieldsSave(exists, includeProperties);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateFieldAsync(Entity item, params Expression<Func<Entity, object>>[] includeProperties)
        {
            return await UpdateFieldAsync(new List<Entity> { item }, includeProperties);
        }

        public virtual SqlParameter[] GetSqlParameters(GetPageR baseSearch)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (PropertyInfo prop in baseSearch.GetType().GetProperties())
            {
                sqlParameters.Add(new SqlParameter(prop.Name, prop.GetValue(baseSearch, null)));
            }
            SqlParameter[] parameters = sqlParameters.ToArray();
            return parameters;
        }
    }
}
