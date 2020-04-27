namespace Application
{
    public static class ExceptionMessages
    {
        public static class Admin
        {
            public const string InvalidRole = "Invalid role.";
            public const string UserNotAddedSuccessfullyToRole = "Something went wrong while adding user to {0} role!";
            public const string UserNotRemovedSuccessfullyFromRole = "Something went wrong while removing user from {0} role!";
        }

        public static class Bid
        {
            public const string InvalidBidAmount = "Invalid bid amount!";
            public const string BiddingNotStartedYet = "Biding for item {0} has not started yet!";
            public const string BiddingHasEnded = "Bidding for item {0} has ended.";
        }

        public static class Item
        {
            public const string CreateItemErrorMessage = "An error occured while creating item.";
            public const string SubCategoryDoesNotExist = "Subcategory does not exist!";
        }

        public static class User
        {
            public const string UserNotCreatedSuccessfully = "User was not created successfully!";
            public const string InvalidCredentials = "Invalid credentials!";
            public const string InvalidRefreshToken = "Invalid token";
        }
    }
}
