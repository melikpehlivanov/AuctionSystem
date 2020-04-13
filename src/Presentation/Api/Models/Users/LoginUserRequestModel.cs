namespace Api.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using Application.Users.Commands.LoginUser;
    using global::Common;
    using global::Common.AutoMapping.Interfaces;

    public class LoginUserRequestModel : IMapWith<LoginUserCommand>
    {
        [Required]
        [RegularExpression(ModelConstants.User.EmailRegex)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
