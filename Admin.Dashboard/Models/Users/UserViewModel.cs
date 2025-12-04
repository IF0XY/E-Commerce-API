namespace Admin.Dashboard.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; }
    }
}
