namespace DoAn.Models
{
    public class UserWithToken : User
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public UserWithToken(User user)
        {
            this.UserId = user.UserId;
            this.FullName = user.FullName;
            this.Phone = user.Phone;
            this.Email = user.Email;
            this.Pass = user.Pass;
        }
    }
}
