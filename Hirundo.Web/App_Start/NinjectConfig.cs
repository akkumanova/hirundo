using System.Web;
using Hirundo.Model;
using Ninject;

namespace Hirundo.Web.App_Start
{
    public static class NinjectConfig
    {
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            kernel.Load(new HirundoModelModule());
        }
    }
}