using Microsoft.EntityFrameworkCore;
using MeetingSystem.IService.Base;
using System.Linq.Expressions;

namespace MeetingSystem.Service.Base
{
    /// <summary>
    /// 泛型服务类，提供针对指定实体类型的完整 CRUD 操作。
    /// 支持条件筛选、排序、投影、批量操作等功能。
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class GenericService<TDbContext, TEntity> : IGenericService<TEntity> where TDbContext : DbContext where TEntity : class, new()
    {
        /// <summary>
        /// 数据库上下文实例
        /// </summary>
        protected readonly TDbContext _dbContext;

        /// <summary>
        /// 实体对应的 DbSet，用于执行实体相关的数据库操作
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 构造函数，初始化数据库上下文和实体 DbSet
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public GenericService(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 检索实体集合，支持条件筛选、排序和投影
        /// </summary>
        /// <param name="predicate">条件表达式，为 null 则不筛选</param>
        /// <param name="order">排序表达式，为 null 则不排序</param>
        /// <param name="selection">投影表达式，为 null 则不投影</param>
        /// <param name="isTracking">是否启用实体变更跟踪，默认为 true</param>
        /// <returns>符合条件的可查询实体集合</returns>
        public virtual IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? order = null, Expression<Func<TEntity, TEntity>>? selection = null, bool isTracking = true)
        {
            // 根据是否需要跟踪来决定查询方式
            IQueryable<TEntity> query = isTracking ? _dbSet : _dbSet.AsNoTracking();

            // 应用条件筛选
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // 应用排序
            if (order != null)
            {
                query = order(query);
            }

            // 应用投影（选择特定字段）
            if (selection != null)
            {
                query = query.Select(selection);
            }
            return query;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns>实体对象，未找到则返回 null</returns>
        public virtual TEntity? Get(object key)
        {
            return _dbSet.Find(key);
        }

        /// <summary>
        /// 获取实体数量（返回 int 类型）
        /// </summary>
        /// <param name="predicate">条件表达式，为 null 则统计全部</param>
        /// <returns>符合条件的实体数量</returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        /// <summary>
        /// 获取实体数量（返回 long 类型，适用于大数据量场景）
        /// </summary>
        /// <param name="predicate">条件表达式，为 null 则统计全部</param>
        /// <returns>符合条件的实体数量</returns>
        public virtual long GetLongCount(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.LongCount() : _dbSet.LongCount(predicate);
        }

        /// <summary>
        /// 判断是否存在符合条件的实体
        /// </summary>
        /// <param name="predicate">条件表达式，为 null 则判断是否有任何记录</param>
        /// <returns>存在返回 true，否则返回 false</returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Any() : _dbSet.Any(predicate);
        }

        /// <summary>
        /// 创建单个实体并保存到数据库
        /// </summary>
        /// <param name="entity">要创建的实体对象</param>
        /// <returns>受影响的行数</returns>
        public virtual int Create(TEntity entity)
        {
            _dbSet.Add(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量创建实体并保存到数据库
        /// </summary>
        /// <param name="entities">要创建的实体集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchCreate(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key">主键值</param>
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
        /// 根据实体对象删除
        /// </summary>
        /// <param name="entity">要删除的实体对象</param>
        /// <returns>受影响的行数</returns>
        public virtual int Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除实体集合
        /// </summary>
        /// <param name="entities">要删除的实体集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchDelete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 根据条件批量删除实体
        /// </summary>
        /// <param name="predicate">条件表达式，为 null 则删除全部</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchDelete(Expression<Func<TEntity, bool>>? predicate = null)
        {
            IQueryable<TEntity> query = predicate == null ? _dbSet : _dbSet.Where(predicate);
            _dbSet.RemoveRange(query);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新实体。若指定了属性列表，则仅更新指定列；否则更新全部列。
        /// </summary>
        /// <param name="entity">要更新的实体对象</param>
        /// <param name="properties">需要更新的列名数组，为空则更新全部</param>
        /// <returns>受影响的行数</returns>
        public virtual int Update(TEntity entity, params string[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                // 指定列更新：先附加实体，再标记指定属性为已修改
                _dbSet.Attach(entity);
                foreach (string property in properties)
                {
                    _dbContext.Entry(entity).Property(property).IsModified = true;
                }
            }
            else
            {
                // 未指定列则更新全部属性
                _dbSet.Update(entity);
            }
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量更新实体。若指定了属性列表，则仅更新指定列；否则更新全部列。
        /// </summary>
        /// <param name="entities">要更新的实体集合</param>
        /// <param name="properties">需要更新的列名数组，为空则更新全部</param>
        /// <returns>受影响的行数</returns>
        public virtual int BatchUpdate(IEnumerable<TEntity> entities, params string[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                // 逐个附加实体并标记指定属性为已修改
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
                // 未指定列则批量更新全部属性
                _dbSet.UpdateRange(entities);
            }
            return _dbContext.SaveChanges();
        }
    }
}
