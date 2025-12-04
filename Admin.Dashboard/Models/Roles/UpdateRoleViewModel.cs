namespace Admin.Dashboard.Models.Roles
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}
