namespace Hirundo.Model.Infrastructure
{
    public interface IUserContextProvider
    {
        UserContext GetCurrentUserContext();

        void SetCurrentUserContext(UserContext userContext);

        void ClearCurrentUserContext();
    }
}
