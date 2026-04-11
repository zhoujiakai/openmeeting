using Newtonsoft.Json;

namespace MeetingSystem.Common.Helper
{
    /// <summary>
    /// JSON格式转换
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        public static string GetJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// SON格式字符转换为T类型的对象
        /// </summary>
        public static T? FromJson<T>(string jsonStr)
        {
            var obj = JsonConvert.DeserializeObject<T>(jsonStr);
            return obj;
        }

        public static string ToJson(this object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//忽略循环引用，如果设置为Error，则遇到循环引用的时候报错（建议设置为Error，这样更规范）
                NullValueHandling = NullValueHandling.Ignore,//忽略值NULL的属性
                DateFormatString = "yyyy-MM-dd HH:mm:ss",//日期格式化，默认的格式也不好看
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()//json中属性开头字母小写的驼峰命名
            };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T? ToObject<T>(this string jsonStr)
        {
            return jsonStr == null ? default : JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
