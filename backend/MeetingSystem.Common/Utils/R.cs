/**
 * uniform return dataform
 */
namespace MeetingSystem.Common.Utils
{
    public class R
    {
        public int Code { get; set; } = 0;
        public string Message { get; set; } = "加载成功";
        public dynamic? Data { get; set; }

        public R SetData(dynamic data)
        {
            this.Data = data;
            return this;
        }
        public R SetMessage(string message)
        {
            this.Message = message;
            return this;
        }

        public R()
        {
        }
        public R(object data)
        {
            Data = data;
        }
        public R(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public R(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        //返回成功
        public R OK()
        {
            return new R();
        }
        public R OK(object data)
        {
            return new R(data);
        }
        //失败
        public R Error()
        {
            return new R(400, "操作失败");
        }
        //用户没有登录
        public R Unauthorized()
        {
            return new R(401, "用户未登录");
        }
        //用户权限不足
        public R Forbidden()
        {
            return new R(403, "用户权限不足");
        }
        //查不到数据
        public R NoContent()
        {
            return new R(204, "查不到数据");
        }
    }
}
