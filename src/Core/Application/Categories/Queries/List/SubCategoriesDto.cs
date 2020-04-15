namespace Application.Categories.Queries.List
{
    using System;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class SubCategoriesDto : IMapWith<SubCategory>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
