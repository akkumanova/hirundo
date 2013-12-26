using System.Web.Mvc;
using System.Web.Routing;
using Hirundo.Model.Utils;
using Ninject;

namespace Hirundo.Web.App_Start
{
    public class MvcConfig
    {
        public static void Register(IKernel kernel)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: string.Empty,
                defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: null,
                url: "login",
                defaults: new { controller = "Account", action = "Login" });

            routes.MapRoute(
               name: null,
               url: "logout",
               defaults: new { controller = "Account", action = "Logout" });
        }
    }
}