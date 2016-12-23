using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using WebApplication.Dependency;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public MvcApplication()
        {
            this.container =
                new WindsorContainer().Install(new DependencyConventions());
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        }
    }
}
