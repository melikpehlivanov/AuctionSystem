namespace Api.Models.Errors
{
    public abstract class BaseErrorModel
    {
        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }
    }
}
