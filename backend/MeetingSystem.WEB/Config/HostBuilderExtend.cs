using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace MeetingSystem.WEB.Config
{
    public static class HostBuilderExtend
    {
        public static void Register(this WebApplicationBuilder app)
        {
            app.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            app.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacModuleRegister());
            });
        }
    }
}
