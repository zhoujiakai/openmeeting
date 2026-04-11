using System.Reflection;
using MeetingSystem.DBFactory.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeetingSystem.DBFactory.DataSeed;

/// <summary>
/// 种子数据初始化器，在应用启动时检查每张表，如果为空则从 JSON 种子文件导入数据
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// 异步种子数据初始化方法，遍历所有 DbSet，空表则从对应 JSON 文件导入
    /// </summary>
    public static async Task SeedAsync(MeetingSystemDbContext context)
    {
        // 定位 DataSeed 目录：优先从运行时目录查找，再尝试项目相对路径
        string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataSeed");
        if (!Directory.Exists(basePath))
        {
            basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
                "MeetingSystem.DBFactory", "DataSeed");
        }

        if (!Directory.Exists(basePath))
            return;

        // 获取 DbContext 中所有 DbSet<> 属性
        var dbSetProperties = typeof(MeetingSystemDbContext).GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .ToList();

        foreach (var prop in dbSetProperties)
        {
            var entityType = prop.PropertyType.GetGenericArguments()[0];
            var dbSetName = prop.Name;
            var jsonPath = Path.Combine(basePath, $"{dbSetName}.json");

            if (!File.Exists(jsonPath))
                continue;

            // 通过反射调用 AnyAsync 判断表是否为空
            var dbSet = prop.GetValue(context);
            var anyMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == nameof(EntityFrameworkQueryableExtensions.AnyAsync) &&
                            m.GetParameters().Length == 1);
            var anyAsync = anyMethod.MakeGenericMethod(entityType);

            var hasDataTask = (Task)anyAsync.Invoke(null, [dbSet!])!;
            await hasDataTask.ConfigureAwait(false);

            var hasDataResult = hasDataTask.GetType().GetProperty("Result")!.GetValue(hasDataTask)!;
            if ((bool)hasDataResult)
                continue;

            // 读取 JSON 文件并反序列化为 List<EntityType>
            var jsonContent = await File.ReadAllTextAsync(jsonPath);
            var listType = typeof(List<>).MakeGenericType(entityType);
            var data = JsonConvert.DeserializeObject(jsonContent, listType);
            if (data == null)
                continue;

            // 调用 DbSet.AddRange
            var addRangeMethod = prop.PropertyType.GetMethod("AddRange");
            addRangeMethod!.Invoke(dbSet, new[] { data });

            await context.SaveChangesAsync();
        }
    }
}
