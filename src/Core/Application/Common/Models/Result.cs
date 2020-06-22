namespace Application.Common.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Result
    {
        private Result(bool succeeded, string error, ErrorType errorType = ErrorType.General)
        {
            this.Succeeded = succeeded;
            this.Error = error;
            this.ErrorType = errorType;
        }

        public bool Succeeded { get; }

        public ErrorType ErrorType { get; }

        public string Error { get; }

        public static Result Success()
            => new Result(true, string.Empty);

        public static Result Failure(string error, ErrorType errorType = ErrorType.General)
            => new Result(false, error, errorType);
    }
}