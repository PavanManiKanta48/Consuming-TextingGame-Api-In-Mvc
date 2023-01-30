namespace Domain.UserModel
{
    public class UserDetailsModel
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string EmailId { get; set; } = null!;

        public string? MobileNo { get; set; }

        public bool IsActive { get; set; }
    }
}
