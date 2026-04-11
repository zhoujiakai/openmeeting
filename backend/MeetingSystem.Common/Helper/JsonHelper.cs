using Newtonsoft.Json;

namespace MeetingSystem.Common.Helper
{
    /// <summary>
    /// JSON格式转换工具类，提供对象与JSON字符串之间的转换方法
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>JSON格式字符串</returns>
        public static string GetJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将JSON格式字符串反序列化为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="jsonStr">JSON格式字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T? FromJson<T>(string jsonStr)
        {
            var obj = JsonConvert.DeserializeObject<T>(jsonStr);
            return obj;
        }

        /// <summary>
        /// 将对象序列化为JSON字符串（扩展方法），忽略循环引用和空值，使用驼峰命名和日期格式化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>格式化后的JSON字符串</returns>
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

        /// <summary>
        /// 将JSON字符串反序列化为指定类型的对象（扩展方法），如果字符串为空则返回默认值
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="jsonStr">JSON格式字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T? ToObject<T>(this string jsonStr)
        {
            return jsonStr == null ? default : JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
