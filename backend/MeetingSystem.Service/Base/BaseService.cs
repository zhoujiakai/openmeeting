using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetingSystem.Service.Base
{
    /// <summary>
    /// 服务基类，提供通用的数据库 CRUD 操作方法。
    /// 所有业务服务类均应继承此基类以获得基础的实体操作能力。
    /// </summary>
    public abstract class BaseService : IBaseService
    {

        /// <summary>
        /// 数据库上下文实例
        /// </summary>
        protected readonly MeetingSystemDbContext _dbContext;

        /// <summary>
        /// 构造函数，通过依赖注入获取数据库上下文
        /// </summary>
        /// <param name="dbContext">会议系统数据库上下文</param>
        public BaseService(MeetingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取指定实体类型的所有记录列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体列表</returns>
        public List<T> ToList<T>() where T : class
        {
            return _dbContext.Set<T>().ToList();
        }

        /// <summary>
        /// 根据主键查找指定实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键值</param>
        /// <returns>实体对象，未找到则返回 null</returns>
        public T? Find<T>(string id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// 根据条件表达式查询实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="express">查询条件表达式</param>
        /// <returns>符合条件的可查询实体集合</returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> express) where T : class
        {
            return _dbContext.Set<T>().Where(express);
        }

        /// <summary>
        /// 插入单个实体并保存到数据库
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要插入的实体对象</param>
        /// <returns>插入后的实体对象</returns>
        public T Insert<T>(T t) where T : class
        {
            _dbContext.Set<T>().Add(t);
            Commit(); // 提交更改到数据库
            return t;
        }

        /// <summary>
        /// 删除单个实体并保存到数据库
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要删除的实体对象</param>
        public void Delete<T>(T t) where T : class
        {
            _dbContext.Set<T>().Remove(t);
            Commit(); // 提交更改到数据库
        }

        /// <summary>
        /// 更新实体并保存到数据库
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">要更新的实体对象</param>
        public void Update<T>(T t) where T : class
        {
            _dbContext.Update(t);
            Commit(); // 提交更改到数据库
        }

        /// <summary>
        /// 提交所有挂起的更改到数据库
        /// </summary>
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量插入实体集合并保存到数据库
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tList">要插入的实体集合</param>
        /// <returns>插入后的实体集合</returns>
        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            _dbContext.Set<T>().AddRange(tList);
            Commit(); // 提交更改到数据库
            return tList;
        }

        /// <summary>
        /// 批量删除实体集合并保存到数据库。
        /// 先将实体附加到上下文中，再执行批量删除。
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tList">要删除的实体集合</param>
        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            // 先将实体附加到上下文，确保可被跟踪和删除
            foreach (var t in tList)
            {
                _dbContext.Set<T>().Attach(t);
            }
            _dbContext.Set<T>().RemoveRange(tList);
            Commit(); // 提交更改到数据库
        }

        /// <summary>
        /// 释放数据库上下文资源
        /// </summary>
        public virtual void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
