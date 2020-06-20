namespace Application.Common.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Result
    {
        private Result(bool succeeded, IEnumerable<string> errors, bool isAccountConfirmationError = false)
        {
            this.Succeeded = succeeded;
            this.Errors = errors.ToArray();
            this.IsAccountConfirmationError = isAccountConfirmationError;
        }

        public bool Succeeded { get; set; }
        
        public bool IsAccountConfirmationError { get; }

        public string[] Errors { get; set; }

        public static Result Success()
            => new Result(true, new string[] { });

        public static Result Failure(IEnumerable<string> errors, bool isAccountConfirmationError = false)
            => new Result(false, errors, isAccountConfirmationError);
    }
}