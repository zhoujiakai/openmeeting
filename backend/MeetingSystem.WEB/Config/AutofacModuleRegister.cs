using Autofac;
using MeetingSystem.IService.Auth;

namespace MeetingSystem.WEB.Config
{
    /// <summary>
    /// Autofac 模块注册器，用于批量注册服务层（Service）及其接口层（IService）的依赖注入映射。
    /// 通过扫描程序集，自动将所有服务类注册到对应的接口。
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        /// <summary>
        /// 重写 Load 方法，在 Autofac 容器构建时自动调用，完成程序集级别的批量注册。
        /// </summary>
        /// <param name="builder">Autofac 容器构建器</param>
        protected override void Load(ContainerBuilder builder)
        {
            // 获取 IService 接口层所在的程序集
            var iServiceAssembly = typeof(IAuthService).Assembly;
            // 获取 Service 实现层所在的程序集
            var serviceAssembly = typeof(MeetingSystem.Service.Auth.AuthService).Assembly;
            // 批量注册：将两个程序集中的类型按其实现的接口进行注册
            builder.RegisterAssemblyTypes(iServiceAssembly, serviceAssembly).AsImplementedInterfaces();
        }
    }
}
