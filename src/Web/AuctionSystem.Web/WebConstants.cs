namespace AuctionSystem.Web
{
    public class WebConstants
    {
        public const string AppMainEmailAddress = "mytestedauctionsystem@gmail.com";

        public const string CategoriesPath = "Resources/SeedFiles/categories.json";
        public const string AdministratorRole = "Administrator";

        public const int ItemsCountPerPage = 24;

        public const string TempDataErrorMessageKey = "ErrorMessage";
        public const string TempDataSuccessMessageKey = "SuccessMessage";

        public const string SearchViewDataKey = "SearchQuery";
        public const int StaticElementsExpirationTimeInDays = 14;

        public const string DefaultPictureUrl = "https://res.cloudinary.com/auctionsystem/image/upload/v1547833155/default-img.jpg";
    }
}