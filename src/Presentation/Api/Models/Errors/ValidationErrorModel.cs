namespace Api.Models.Errors
{
    using System.Collections.Generic;

    public class ValidationErrorModel : BaseErrorModel
    {
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
