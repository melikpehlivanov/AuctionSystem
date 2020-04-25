namespace MvcWeb.ViewModels.Item
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Application.Items.Commands.CreateItem;
    using Common;
    using Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ItemCreateBindingModel : IMapWith<CreateItemCommand>, IValidatableObject
    {
        private const string StartTimeBeforeCurrentTimeError = "The start time must be after the current time";

        private const string EndTimeBeforeStartTimeError = "The end time must be after the start time";

        [Required]
        [MaxLength(ModelConstants.Item.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ModelConstants.Item.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Starting price")]
        [Range(typeof(decimal), ModelConstants.Item.MinStartingPriceAsString, ModelConstants.Item.MaxStartingPriceAsString)]
        public decimal StartingPrice { get; set; }

        [Required]
        [Display(Name = "Minimal price increase allowed")]
        [Range(typeof(decimal), ModelConstants.Item.MinMinIncreaseAsString, ModelConstants.Item.MaxMinIncreaseAsString)]
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

        public ICollection<IFormFile> PictureFormFiles { get; set; }

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