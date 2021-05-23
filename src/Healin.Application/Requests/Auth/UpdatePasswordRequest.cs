namespace Healin.Application.Requests.Auth
{
    public class UpdatePasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
