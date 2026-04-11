namespace MeetingSystem.Common.Helper
{
    /// <summary>
    /// Redis操作帮助类，用于缓存数据的读写操作
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// Redis操作的单例实例
        /// </summary>
        public static RedisHelper? Instance { get; private set; }

    }
}
