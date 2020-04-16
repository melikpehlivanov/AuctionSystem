namespace MvcWeb.ViewModels.Category
{
    using System;
    using Application.Categories.Queries.List;
    using Common.AutoMapping.Interfaces;

    public class SubCategoryViewModel : IMapWith<SubCategoriesDto>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
