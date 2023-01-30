using ConsumingWebApi.Models;

namespace Domain.UserModel
{
    public class LoginUserResponseModel : BaseResponse
    {
        public int userId { get; set; }
        public string? Token { get; set; }
    }
}
