namespace Application.Categories.Queries.List
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class ListCategoriesResponseModel : IMapWith<Category>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SubCategoriesDto> SubCategories { get; set; }
    }
}
