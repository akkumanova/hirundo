namespace Hirundo.Model
{
    using Hirundo.Model.Data;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Repositories.CommentRepository;
    using Hirundo.Model.Repositories.ImagesRepository;
    using Hirundo.Model.Repositories.UserRepository;
    using Ninject.Extensions.NamedScope;
    using Ninject.Modules;

    public class HirundoModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserContextProvider>().To<UserContextProvider>();
            Bind<IHirundoContext>().To<HirundoContext>();
            Bind<IUserRepository>().To<UserRepository>().InCallScope();
            Bind<ICommentRepository>().To<CommentRepository>().InCallScope();
            Bind<IImageRepository>().To<ImageRepository>();
        }
    }
}
