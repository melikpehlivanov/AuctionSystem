namespace AuctionSystem.Services.Models.Item
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Microsoft.AspNetCore.Http;

    public class ItemCreateServiceModel : BaseItemServiceModel
    {
        [Required]
        [MaxLength(ModelConstants.Item.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ModelConstants.Item.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), ModelConstants.Item.MinStartingPrice, ModelConstants.Item.MaxStartingPrice)]
        public decimal StartingPrice { get; set; }

        [Required]
        [Range(typeof(decimal), ModelConstants.Item.MinMinIncrease, ModelConstants.Item.MaxMinIncrease)]
        public decimal MinIncrease { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

        [Required]
        public string UserName { get; set; }
        
        public ICollection<IFormFile> PictFormFiles { get; set; }
    }
}