namespace Hirundo.Web
{
    using Hirundo.Web.Mappers;
    using Ninject.Modules;

    public class HirundoWebIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().To<CommentDataMapper>();
            Bind<IMapper>().To<CommentDetailsMapper>();
            Bind<IMapper>().To<ReplyMapper>();
            Bind<IMapper>().To<UserMapper>();
            Bind<IMapper>().To<UserDataMapper>();
            Bind<IMapper>().To<UserProfileMapper>();
        }
    }
}