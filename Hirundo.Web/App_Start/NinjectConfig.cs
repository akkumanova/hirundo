namespace Hirundo.Web.App_Start
{
    using System.Web;
    using Hirundo.Model;
    using Ninject;

    public static class NinjectConfig
    {
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            kernel.Load(new HirundoModelModule());
        }
    }
}