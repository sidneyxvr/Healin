namespace Healin.Application.Responses.Auth
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenResponse UserToken { get; set; }
    }
}
