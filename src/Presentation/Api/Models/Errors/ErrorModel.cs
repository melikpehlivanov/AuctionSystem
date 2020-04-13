namespace Api.Models.Errors
{
    public class ErrorModel
    {
        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

        public string Error { get; set; }
    }
}
