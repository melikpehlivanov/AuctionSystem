namespace Application.UnitTests.Setup
{
    using System;

    public static class DataConstants
    {
        public static readonly string SampleUserId = "8490931b-67b9-4784-a231-99c898636ee6";
        public static readonly string SampleAdminUserId = "0ef2317e-326e-4457-8cf9-7ee57ea4385f";

        public static readonly Guid SampleItemId = Guid.Parse("342b77c3-89de-4b37-9969-baa91c1573f0");
        public const string SampleItemTitle = "SampleTitle";
        public const string SampleItemDescription = "Very cool item";
        public const string SampleItemUsername = "TestUser";
        public const string SampleItemUserFullName = "Test Test";
        public const decimal SampleItemStartingPrice = 300;
        public const decimal SampleItemMinIncrease = 10;
        public static readonly DateTime SampleItemStartTime = DateTime.MinValue;
        public static readonly DateTime SampleItemEndTime = DateTime.MaxValue;

        public static readonly Guid SampleSubCategoryId = Guid.Parse("386c2db8-5b8a-4744-8f60-7fcd3b1c7653");
        public static readonly Guid SampleCategoryId = Guid.Parse("fe712d64-226e-4007-8bf2-76c0ebb12e96");

        public static readonly Guid SamplePictureId = Guid.Parse("73716e91-4989-488e-8f77-dcf46b126c3b");
    }
}
