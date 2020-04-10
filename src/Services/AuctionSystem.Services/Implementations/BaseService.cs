namespace AuctionSystem.Services.Implementations
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Data;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public abstract class BaseService
    {
        protected readonly IMapper mapper;
        protected readonly AuctionSystemDbContext Context;

        protected BaseService(IMapper mapper, AuctionSystemDbContext context)
        {
            this.mapper = mapper;
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
