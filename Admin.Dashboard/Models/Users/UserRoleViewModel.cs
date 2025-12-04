using Admin.Dashboard.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace Admin.Dashboard.Models.Users
{
    public class UserRoleViewModel
    {
        [Display(Name ="User Id")]
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public IList<UpdateRoleViewModel> Roles { get; set; }
    }
}
