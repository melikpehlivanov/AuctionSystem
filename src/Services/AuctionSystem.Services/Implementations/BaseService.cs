namespace AuctionSystem.Services.Implementations
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using Interfaces;

    public abstract class BaseService : IService
    {
        protected readonly AuctionSystemDbContext Context;

        protected BaseService(AuctionSystemDbContext context)
        {
            this.Context = context;
        }

        protected bool IsEntityStateValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(model, validationContext, validationResults,
                validateAllProperties: true);
        }
    }
}