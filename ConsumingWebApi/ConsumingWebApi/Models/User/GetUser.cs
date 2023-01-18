namespace ConsumingWebApi.Models.User
{
    public class GetUser
    {
        public string? UserName { get; set; }

        public string EmailId { get; set; } = null!;
        public string? MobileNo { get; set; }
    }
}

