namespace Application.Items.Queries.List
{
    using System;

    public class ListAllItemsQueryFilter
    {
        public string Title { get; set; }

        public string UserId { get; set; }

        public decimal? MinPrice { get; set; }
        
        public decimal? MaxPrice { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool GetLiveItems { get; set; }

        public int? MinimumPicturesCount { get; set; }

        public Guid SubCategoryId { get; set; }
    }
}