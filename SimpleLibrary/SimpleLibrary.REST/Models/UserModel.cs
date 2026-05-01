namespace SimpleLibrary.REST.Models
{
    public class UserCreateModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
    }
    public class UserRoleUpdateModel
    {
        public int RoleId { get; set; }
    }

    public class UserProfileUpdateModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
    }

    public class UserResponseModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}