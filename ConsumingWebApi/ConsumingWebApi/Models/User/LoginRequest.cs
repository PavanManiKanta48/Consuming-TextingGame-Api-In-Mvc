using System.ComponentModel.DataAnnotations;

namespace ConsumingWebApi.Models.User
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "email id required!")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "email requires: @,.com,.co")]
        public string? EmailId { get; set; }
        
        public string? Password { get; set; }
    }
}
