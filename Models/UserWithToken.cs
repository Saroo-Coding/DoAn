namespace DoAn.Models
{
    public class UserWithToken : User
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public UserWithToken(User user)
        {
            this.UserId = user.UserId;
        }
    }
}
