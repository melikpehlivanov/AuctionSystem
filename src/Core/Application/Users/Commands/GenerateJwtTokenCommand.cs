namespace Application.Users.Commands
{
    using MediatR;

    public class GenerateJwtTokenCommand : IRequest<string>
    {
        public GenerateJwtTokenCommand(string userId, string username)
        {
            this.UserId = userId;
            this.Username = username;
        }

        public string UserId { get; }

        public string Username { get; }
    }
}
