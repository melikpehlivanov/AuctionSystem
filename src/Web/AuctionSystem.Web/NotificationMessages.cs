namespace AuctionSystem.Web
{
    public static class NotificationMessages
    {
        public const string ItemCreateError = "An error occured while creating item";
        public const string ItemCreated = "Item created";
        public const string ItemNotFound = "Oops! Looks like the item you're searching for actually does not exist.";
        public const string BidNotFound = "Oops! Looks like the bid you're searching for actually does not exist.";
        
        public const string EmailSentSuccessfully = "Email was sent successfully";

        public const string SearchQueryTooShort = "Please enter at least 3 characters";
        public const string SearchNoItems = "No results";
    }
}