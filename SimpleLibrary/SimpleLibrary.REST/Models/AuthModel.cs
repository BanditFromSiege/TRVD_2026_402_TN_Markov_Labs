namespace SimpleLibrary.REST.Models
{
    public class AuthLoginRequestModel
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class AuthRegisterRequestModel
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class AuthResponseModel
    {
        public string Token { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;
    }
}