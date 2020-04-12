﻿namespace Common
{
    public static class ModelConstants
    {
        public static class User
        {
            public const int FullNameMaxLength = 50;
        }

        public static class Bid
        {
            public const decimal MinAmount = 0.01m;
            public const decimal MaxAmount = 999999999999;
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
            public const decimal MaxStartingPrice = decimal.MaxValue;
            public const decimal MinMinIncrease = 0.01m;
            public const decimal MaxMinIncrease = decimal.MaxValue;
        }

        public static class SubCategory
        {
            public const int NameMaxLength = 50;
        }
    }
}