using System.Net;

namespace ConsumingWebApi.Models
{
    public class BaseResponse
    {
        public int userId { get; set; }
        public string? Token { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
