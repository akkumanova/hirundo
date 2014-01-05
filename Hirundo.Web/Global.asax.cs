namespace Hirundo.Web
{
    using System.Web.Http;
    using Hirundo.Web.App_Start;
    using Ninject;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IKernel kernel = new StandardKernel();
            NinjectConfig.RegisterServices(kernel);

            MvcConfig.Register(kernel);
            WebApiConfig.Register(kernel, GlobalConfiguration.Configuration);
        }
    }
}