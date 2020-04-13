namespace Api.Models
{
    public class ErrorModel
    {
        public string Error { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }
    }
}
