using System.ComponentModel.DataAnnotations;

namespace Admin.Dashboard.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role Name Is Required")]
        [StringLength(256, ErrorMessage = "Role Name Must Be Less Than 256 Chars")]
        public string Name { get; set; } = null!;
    }
}
