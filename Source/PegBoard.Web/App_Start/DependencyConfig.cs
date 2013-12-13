using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace PegBoard.Web.App_Start
{
    public class DependencyConfig
    {
        // see https://code.google.com/p/autofac/wiki/MvcIntegration
        public static void RegisterAll()
        {
            var currentAssembly = typeof(MvcApplication).Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterControllers(currentAssembly);
            builder.RegisterApiControllers(currentAssembly);
            builder.RegisterFilterProvider();
            builder.RegisterAssemblyTypes(currentAssembly).AsImplementedInterfaces();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); ;
        }
    }
}