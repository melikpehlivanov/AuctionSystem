namespace Application
{
    public static class ExceptionMessages
    {
        public static class Admin
        {
            public const string InvalidRole = "Invalid role.";
            public const string UserNotAddedSuccessfullyToRole = "Something went wrong while adding user to {0} role!";

            public const string UserNotRemovedSuccessfullyFromRole =
                "Something went wrong while removing user from {0} role!";
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
            public const string UserNotFound = "User not found";

            public const string AccountLockout =
                "Your account has been locked out due to too many invalid login attempts. Please try again.";

            public const string ConfirmAccount = "Please confirm your account";
            public const string EmailVerificationFailed = "Account confirmation failed. Please try again later.";
            
            public const string CannotRemoveSelfFromRole = "You can not remove yourself from role {0}!";
            public const string NotInRole = "{0} is not {1}.";
        }
    }
}