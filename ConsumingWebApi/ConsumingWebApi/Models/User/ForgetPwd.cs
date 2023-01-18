using System.ComponentModel.DataAnnotations;

namespace ConsumingWebApi.Models.User
{
    public class ForgetPwd
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "email requires: @,.com,.co")]
        public string? EmailId { get; set; }
        [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Numbers,\r\nLowercase,\r\nUppercase,\r\nSpecial (viz. !@#$%&/=?_.)")]
        public string? Password { get; set; }
    }
}
