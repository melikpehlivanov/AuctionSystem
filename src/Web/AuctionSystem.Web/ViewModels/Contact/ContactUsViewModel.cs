namespace AuctionSystem.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;

    public class ContactUsViewModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public string Subject { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(20)]
        public string Message { get; set; }
    }
}
