namespace Common
{
    public static class ModelConstants
    {
        public static class User
        {
            public const int FullNameMaxLength = 50;
        }

        public static class Bid
        {
            public const string MinAmount = "0.01";
            public const string MaxAmount = "999999999999";
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class Item
        {
            public const int TitleMaxLength = 120;
            public const int DescriptionMaxLength = 500;
            public const string MinStartingPrice = "0.01";
            public const string MaxStartingPrice = "79228162514264337593543950335";
            public const string MinMinIncrease = "0.01";
            public const string MaxMinIncrease = "79228162514264337593543950335";
        }

        public static class SubCategory
        {
            public const int NameMaxLength = 50;
        }
    }
}
