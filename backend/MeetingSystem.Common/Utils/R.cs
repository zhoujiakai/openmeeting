/**
 * uniform return dataform
 */
namespace MeetingSystem.Common.Utils
{
    /// <summary>
    /// 统一返回结果类，用于封装API接口的返回数据
    /// </summary>
    public class R
    {
        /// <summary>
        /// 状态码，0表示成功，其他表示不同的错误类型
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 返回消息，描述请求处理结果
        /// </summary>
        public string Message { get; set; } = "加载成功";

        /// <summary>
        /// 返回的数据内容，动态类型
        /// </summary>
        public dynamic? Data { get; set; }

        /// <summary>
        /// 设置返回数据
        /// </summary>
        /// <param name="data">要返回的数据</param>
        /// <returns>当前R对象，支持链式调用</returns>
        public R SetData(dynamic data)
        {
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 设置返回消息
        /// </summary>
        /// <param name="message">要设置的消息内容</param>
        /// <returns>当前R对象，支持链式调用</returns>
        public R SetMessage(string message)
        {
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 默认构造函数，返回成功状态
        /// </summary>
        public R()
        {
        }

        /// <summary>
        /// 带数据的构造函数
        /// </summary>
        /// <param name="data">返回的数据</param>
        public R(object data)
        {
            Data = data;
        }

        /// <summary>
        /// 带状态码和消息的构造函数
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">返回消息</param>
        public R(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 带状态码、消息和数据的构造函数
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回的数据</param>
        public R(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// 返回成功结果（无数据）
        /// </summary>
        /// <returns>成功的R对象</returns>
        public R OK()
        {
            return new R();
        }

        /// <summary>
        /// 返回成功结果（带数据）
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns>成功的R对象</returns>
        public R OK(object data)
        {
            return new R(data);
        }

        /// <summary>
        /// 返回失败结果
        /// </summary>
        /// <returns>失败的R对象</returns>
        public R Error()
        {
            return new R(400, "操作失败");
        }

        /// <summary>
        /// 返回未授权结果（用户未登录）
        /// </summary>
        /// <returns>未授权的R对象</returns>
        public R Unauthorized()
        {
            return new R(401, "用户未登录");
        }

        /// <summary>
        /// 返回禁止访问结果（用户权限不足）
        /// </summary>
        /// <returns>禁止访问的R对象</returns>
        public R Forbidden()
        {
            return new R(403, "用户权限不足");
        }

        /// <summary>
        /// 返回无内容结果（查不到数据）
        /// </summary>
        /// <returns>无内容的R对象</returns>
        public R NoContent()
        {
            return new R(204, "查不到数据");
        }
    }
}
