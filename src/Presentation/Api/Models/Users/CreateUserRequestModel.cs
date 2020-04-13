namespace Api.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using Application.Users.Commands.CreateUser;
    using global::Common;
    using global::Common.AutoMapping.Interfaces;

    public class CreateUserRequestModel : IMapWith<CreateUserCommand>
    {
        [Required]
        [RegularExpression(ModelConstants.User.EmailRegex)]
        public string Email { get; set; }

        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
