using System.Linq.Expressions;

namespace MeetingSystem.IService.Base
{
    public interface IBaseService : IDisposable
    {
        // 查询
        T? Find<T>(string id) where T : class;

        IQueryable<T> Query<T>(Expression<Func<T, bool>> express) where T : class;

        // 增加
        T Insert<T>(T t) where T : class;

        IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;

        // 删除
        void Delete<T>(T t) where T : class;

        void Delete<T>(IEnumerable<T> tList) where T : class;

        //更新
        void Update<T>(T t) where T : class;

        void Commit();
    }
}
