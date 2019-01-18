namespace AuctionSystem.Web.ViewModels.Item
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Models.Item;

    public class ItemCreateBindingModel : IMapWith<ItemCreateServiceModel>, IValidatableObject
    {
        private const string StartTimeBeforeCurrentTimeError = "The start time must be after the current time";

        private const string EndTimeBeforeStartTimeError = "The end time must be after the start time";

        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Starting price")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal StartingPrice { get; set; }

        [Required]
        [Display(Name = "Minimal price increase allowed")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal MinIncrease { get; set; }

        [Required]
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string SubCategoryId { get; set; }

        public ICollection<IFormFile> PictFormFiles { get; set; }

        public IEnumerable<SelectListItem> SubCategories { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.StartTime.ToUniversalTime() <= DateTime.UtcNow)
            {
                yield return new ValidationResult(StartTimeBeforeCurrentTimeError,
                    new[] {nameof(this.StartTime)});
            }

            if (this.EndTime.ToUniversalTime() <= this.StartTime.ToUniversalTime())
            {
                yield return new ValidationResult(EndTimeBeforeStartTimeError,
                    new[] {nameof(this.EndTime)});
            }
        }
    }
}