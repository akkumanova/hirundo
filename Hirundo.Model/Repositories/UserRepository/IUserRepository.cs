using Hirundo.Model.Models;

namespace Hirundo.Model.Repositories.UserRepository
{
    public interface IUserRepository
    {
        User FindByUsername(string username);
    }
}
