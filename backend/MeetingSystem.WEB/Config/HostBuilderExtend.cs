using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace MeetingSystem.WEB.Config
{
    /// <summary>
    /// WebApplicationBuilder 扩展类，用于将 Autofac 作为依赖注入容器替换默认容器。
    /// </summary>
    public static class HostBuilderExtend
    {
        /// <summary>
        /// 注册 Autofac 容器，替换 .NET 默认的 DI 容器，并加载自定义模块。
        /// </summary>
        /// <param name="app">Web 应用构建器实例</param>
        public static void Register(this WebApplicationBuilder app)
        {
            // 使用 AutofacServiceProviderFactory 替换默认的服务提供者工厂
            app.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            // 配置 Autofac 容器，注册自定义模块
            app.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacModuleRegister());
            });
        }
    }
}
