namespace Api.SwaggerExamples
{
    public static class SwaggerDocumentation
    {
        public const string UnauthorizedDescriptionMessage = "Indicates that users need to be authorized in order to access it";

        public static class ItemConstants
        {
            public const string NotFoundDescriptionMessage = "Indicates that item does not exist";
            public const string BadRequestDescriptionMessage = "Indicates that the provided data is invalid or something else went wrong";

            public const string SuccessfulGetRequestMessage = "Returns all items";
            public const string SuccessfulGetRequestWithIdDescriptionMessage = "Successfully found item and returns it";

            public const string SuccessfulPostRequestDescriptionMessage = "Creates item successfully and returns the Id of the item";
            public const string SuccessfulPutRequestDescriptionMessage = "Item is updated successfully";
            public const string SuccessfulDeleteRequestDescriptionMessage = "Indicates that item is deleted successfully";
        }

        public static class PictureConstants
        {
            public const string BadRequestDescriptionMessage =
                "Indicates that picture either does not exist or user does not have permission to delete it";

            public const string SuccessfulGetRequestDescriptionMessage =
                "Pictures were uploaded successfully and their corresponding data will be returned";

            public const string SuccessfulDeleteRequestDescriptionMessage = "Picture is deleted successfully";

            public const string SuccessfulGetPictureDetailsRequestDescriptionMessage = "Pictures were retrieved successfully and returned";
        }

        public static class CategoriesConstants
        {
            public const string SuccessfulGetRequestMessage = "Returns all categories with their subcategories";
            public const string BadRequestDescriptionMessage = "Indicates that there are not any categories";
        }

        public static class IdentityConstants
        {
            public const string SuccessfulRegisterRequestDescriptionMessage = "Indicates that the user is created";
            public const string BadRequestOnRegisterDescriptionMessage = "Indicates that the user data is invalid";

            public const string SuccessfulLoginRequestDescriptionMessage = "Indicates that everything went ok and returns jwt token";
            public const string BadRequestOnLoginDescriptionMessage = "Indicates that the user credentials are invalid";


            public const string SuccessfulTokenRefreshRequestDescriptionMessage = 
                "Indicates that everything went ok and returns new jwt & refresh token";
            public const string BadRequestOnTokenRefreshDescriptionMessage = "Indicates that the provided token is invalid";
        }

        public static class BidConstants
        {
            public const string SuccessfulPostRequestDescriptionMessage = "Indicates that bid is created successfully";
            public const string BadRequestOnPostRequestDescriptionMessage = "Indicates that the data is invalid";
            public const string NotFoundOnPostRequestDescriptionMessage = "Indicates that such item does not exist";

            public const string GetHighestBidDescriptionMessage = "Retrieves the highest bid otherwise returns null";
        }

        public static class AdminConstants
        {
            public const string SuccessfulGetRequestDescriptionMessage = "Retrieves all users and returns them as a collection";
            public const string SuccessfulPostRequestDescriptionMessage = "Indicates that user is successfully added to the given role";

            public const string BadRequestDescriptionMessage = "Indicates that either the role or user data is invalid";

            public const string SuccessfulDeleteRequestDescriptionMessage = "Indicates that user is no longer administrator";
        }
    }
}