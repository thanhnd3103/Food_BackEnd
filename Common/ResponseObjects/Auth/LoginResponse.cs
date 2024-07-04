namespace Common.ResponseObjects.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
    }
}
