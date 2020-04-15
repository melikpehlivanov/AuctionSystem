namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        bool IsAuthenticated { get; }

        //TODO: Don't trust claims... Check later in database if user is really admin
        bool IsAdmin { get; }
    }
}
