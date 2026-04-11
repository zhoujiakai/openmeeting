using Autofac;
using MeetingSystem.IService.Auth;

namespace MeetingSystem.WEB.Config
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var iServiceAssembly = typeof(IAuthService).Assembly;
            var serviceAssembly = typeof(MeetingSystem.Service.Auth.AuthService).Assembly;
            builder.RegisterAssemblyTypes(iServiceAssembly, serviceAssembly).AsImplementedInterfaces();
        }
    }
}
