using Hirundo.Model.Data;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Repositories.UserRepository;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;

namespace Hirundo.Model
{
    public class HirundoModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserContextProvider>().To<UserContextProvider>();
            Bind<IMongoContext>().To<MongoContext>();
            Bind<IUserRepository>().To<UserRepository>().InCallScope();
        }
    }
}
