namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;
    using global::Common;

    public class Item : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

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
        public bool IsEmailSent { get; set; } = false;
        
        [Required]
        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<Bid> Bids { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}
