using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeetingSystem.Service.Base
{
    public abstract class BaseService : IBaseService
    {

        protected readonly MeetingSystemDbContext _dbContext;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseService(MeetingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<T> ToList<T>() where T : class
        {
            return _dbContext.Set<T>().ToList();
        }
        public T? Find<T>(string id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> express) where T : class
        {
            return _dbContext.Set<T>().Where(express);
        }

        public T Insert<T>(T t) where T : class
        {
            _dbContext.Set<T>().Add(t);
            Commit();
            return t;
        }

        public void Delete<T>(T t) where T : class
        {
            _dbContext.Set<T>().Remove(t);
            Commit();
        }

        public void Update<T>(T t) where T : class
        {
            _dbContext.Update(t);
            Commit();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            _dbContext.Set<T>().AddRange(tList);
            Commit();
            return tList;
        }

        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                _dbContext.Set<T>().Attach(t);
            }
            _dbContext.Set<T>().RemoveRange(tList);
            Commit();
        }
        public virtual void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
