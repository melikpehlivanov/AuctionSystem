namespace Application.Users.Commands
{
    using MediatR;

    public class GenerateJwtTokenCommand : IRequest<string>
    {
        public GenerateJwtTokenCommand(string userId, string username, string secret)
        {
            this.UserId = userId;
            this.Username = username;
            this.Secret = secret;
        }

        public string UserId { get; }

        public string Username { get; }

        public string Secret { get; }
    }
}
