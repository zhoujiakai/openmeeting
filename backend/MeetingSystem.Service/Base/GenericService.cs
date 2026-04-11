using Microsoft.EntityFrameworkCore;
using MeetingSystem.IService.Base;
using System.Linq.Expressions;

namespace MeetingSystem.Service.Base
{
    public class GenericService<TDbContext, TEntity> : IGenericService<TEntity> where TDbContext : DbContext where TEntity : class, new()
    {
        protected readonly TDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public GenericService(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 检索实体集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="selection">投影表达式</param>
        /// <param name="isTracking">是否跟踪实体</param>
        /// <returns>实体集合</returns>
        public virtual IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? order = null, Expression<Func<TEntity, TEntity>>? selection = null, bool isTracking = true)
        {
            IQueryable<TEntity> query = isTracking ? _dbSet : _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (order != null)
            {
                query = order(query);
            }
            if (selection != null)
            {
                query = query.Select(selection);
            }
            return query;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>实体</returns>
        public virtual TEntity? Get(object key)
        {
            return _dbSet.Find(key);
        }

        /// <summary>
        /// 获取实体数量(int)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体数量</returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        /// <summary>
        /// 获取实体数量(long)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体数量</returns>
        public virtual long GetLongCount(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.LongCount() : _dbSet.LongCount(predicate);
        }

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>是否存在</returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Any() : _dbSet.Any(predicate);
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的行数</returns>
        public virtual int Create(TEntity entity)
        {
            _dbSet.Add(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量创建实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchCreate(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>受影响的行数</returns>
        public virtual int Delete(object key)
        {
            TEntity? entity = _dbSet.Find(key);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的行数</returns>
        public virtual int Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchDelete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchDelete(Expression<Func<TEntity, bool>>? predicate = null)
        {
            IQueryable<TEntity> query = predicate == null ? _dbSet : _dbSet.Where(predicate);
            _dbSet.RemoveRange(query);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新的列</param>
        /// <returns>受影响的行数</returns>
        public virtual int Update(TEntity entity, params string[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                _dbSet.Attach(entity);
                foreach (string property in properties)
                {
                    _dbContext.Entry(entity).Property(property).IsModified = true;
                }
            }
            else
            {
                _dbSet.Update(entity);
            }
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新的列</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchUpdate(IEnumerable<TEntity> entities, params string[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                foreach (TEntity entity in entities)
                {
                    _dbSet.Attach(entity);
                    foreach (string property in properties)
                    {
                        _dbContext.Entry(entity).Property(property).IsModified = true;
                    }
                }
            }
            else
            {
                _dbSet.UpdateRange(entities);
            }
            return _dbContext.SaveChanges();
        }
    }
}
