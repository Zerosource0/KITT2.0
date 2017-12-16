using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using KITT.Facade.Initialization;
using KITT.Facade.Interfaces;
using Owin;

namespace KITT
{
    public class IocConfig
    {
        public static void Configure(IAppBuilder app)
        {

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("KITT")).ToArray();
            //register all types implementing an interface.

            builder.RegisterType<DatabaseSchemaHandler>().As<IDatabaseSchemaHandler>();
            builder.RegisterType<Facade.Facade>().As<IFacade>();

            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces()
            //    .AsSelf();

            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.StartsWith("Generic"))
            //    .AsImplementedInterfaces()
            //    .AsSelf();

            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.EndsWith("FileHandler"))
            //    .AsImplementedInterfaces()
            //    .AsSelf();

            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.StartsWith("Upload"))
            //    .AsImplementedInterfaces()
            //    .AsSelf();

            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.StartsWith("Download"))
            //    .AsImplementedInterfaces()
            //    .AsSelf();

            
            var container = builder.Build();


            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

        }

    }
}