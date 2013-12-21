using Hirundo.Web.App_Start;
using Ninject;

namespace Hirundo.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IKernel kernel = new StandardKernel();
            NinjectConfig.RegisterServices(kernel);

            MvcConfig.Register(kernel);
        }
    }
}