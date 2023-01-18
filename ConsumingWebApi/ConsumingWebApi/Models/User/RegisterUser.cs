using Microsoft.CodeAnalysis.Completion;
using System.ComponentModel.DataAnnotations;

namespace ConsumingWebApi.Models.User
{
    public class RegisterUser
    {
       public string Name { get; set; }
        
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "email requires: @,.com,.co")]
        public string EmailId { get; set; } = null!;

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character:")]
        public string? Password { get; set; }
        
        [RegularExpression(@"^([0-9]{10})",ErrorMessage ="Please enter a 10 digit valid number")]
        public string? MobileNo { get; set; }
        [Compare("Password",ErrorMessage ="Confirm Password was not matched")]
        public string? ConfirmPassword { get; set; }
    }
}
