using System.Linq.Expressions;

namespace MeetingSystem.IService.Base
{
    public interface IGenericService<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 检索实体集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="selection">投影表达式</param>
        /// <param name="isTracking">是否跟踪实体</param>
        /// <returns>实体集合</returns>
        IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? order = null, Expression<Func<TEntity, TEntity>>? selection = null, bool isTracking = true);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>实体</returns>
        TEntity? Get(object key);

        /// <summary>
        /// 获取实体数量(int)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体数量</returns>
        int GetCount(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 获取实体数量(long)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体数量</returns>
        long GetLongCount(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>是否存在</returns>
        bool Exists(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的行数</returns>
        int Create(TEntity entity);

        /// <summary>
        /// 批量创建实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的行数</returns>
        int BatchCreate(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>受影响的行数</returns>
        int Delete(object key);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的行数</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的行数</returns>
        int BatchDelete(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>受影响的行数</returns>
        int BatchDelete(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新的列</param>
        /// <returns>受影响的行数</returns>
        int Update(TEntity entity, params string[] properties);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新的列</param>
        /// <returns>受影响的行数</returns>
        int BatchUpdate(IEnumerable<TEntity> entities, params string[] properties);
    }
}
