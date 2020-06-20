namespace Application.Users.Commands.ConfirmEmail
{
    using MediatR;

    public class ConfirmEmailCommand : IRequest
    {
        public string Code { get; set; }

        public string Email { get; set; }
    }
}