using System.Linq.Expressions;

namespace MeetingSystem.IService.Base
{
    /// <summary>
    /// 基础服务接口，提供通用的增删改查操作
    /// </summary>
    public interface IBaseService : IDisposable
    {
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <returns>实体对象，未找到时返回null</returns>
        T? Find<T>(string id) where T : class;

        /// <summary>
        /// 根据条件查询实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="express">查询条件表达式</param>
        /// <returns>符合条件的实体查询集合</returns>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> express) where T : class;

        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要插入的实体</param>
        /// <returns>插入后的实体</returns>
        T Insert<T>(T t) where T : class;

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tList">要插入的实体集合</param>
        /// <returns>插入后的实体集合</returns>
        IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要删除的实体</param>
        void Delete<T>(T t) where T : class;

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tList">要删除的实体集合</param>
        void Delete<T>(IEnumerable<T> tList) where T : class;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要更新的实体</param>
        void Update<T>(T t) where T : class;

        /// <summary>
        /// 提交所有变更到数据库
        /// </summary>
        void Commit();
    }
}
