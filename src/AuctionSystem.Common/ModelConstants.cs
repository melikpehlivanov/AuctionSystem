namespace AuctionSystem.Common
{
    public class ModelConstants
    {
        public class User
        {
            public const int FullNameMaxLength = 50;
        }

        public class Bid
        {
            public const string MinAmount = "0.01";
            public const string MaxAmount = "999999999999";
        }

        public class Category
        {
            public const int NameMaxLength = 50;
        }

        public class Item
        {
            public const int TitleMaxLength = 120;
            public const int DescriptionMaxLength = 500;
            public const string MinStartingPrice = "0.01";
            public const string MaxStartingPrice = "79228162514264337593543950335";
            public const string MinMinIncrease = "0.01";
            public const string MaxMinIncrease = "79228162514264337593543950335";
        }

        public class SubCategory
        {
            public const int NameMaxLength = 50;
        }
    }
}
