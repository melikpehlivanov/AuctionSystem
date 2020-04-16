namespace Common
{
    public static class ModelConstants
    {
        public static class User
        {
            public const string EmailRegex =
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            public const int FullNameMaxLength = 50;
        }

        public static class Bid
        {
            public const decimal MinAmount = 0.01m;
            public const string MinAmountAsString = "0.01";
            public const decimal MaxAmount = 999999999999;
            public const string MaxAmountAsString = "999999999999";
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class Item
        {
            public const int TitleMaxLength = 120;
            public const int DescriptionMaxLength = 500;
            public const decimal MinStartingPrice = 0.01m;
            public const string MinStartingPriceAsString = "0.01";

            public const decimal MaxStartingPrice = decimal.MaxValue;
            public const string MaxStartingPriceAsString = "79228162514264337593543950335";


            public const decimal MinMinIncrease = 0.01m;
            public const string MinMinIncreaseAsString = "0.01";

            public const decimal MaxMinIncrease = decimal.MaxValue;
            public const string MaxMinIncreaseAsString = "79228162514264337593543950335";
        }

        public static class SubCategory
        {
            public const int NameMaxLength = 50;
        }
    }
}