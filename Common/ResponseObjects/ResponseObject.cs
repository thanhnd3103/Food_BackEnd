using System.Net;

namespace Common.ResponseObjects
{
    public class ResponseObject
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Result { get; set; }
    }
}
