using System.ComponentModel.DataAnnotations;
namespace Admin.Dashboard.Models.ProductBrandAndTypeViewModels
{
    public class CreatedBrandAndTypeViewModel
    {
        [Required(ErrorMessage = "Brand Name Is Required")]
        [StringLength(256, ErrorMessage = "Brand Name Must Be Less Than 256 Chars")]
        public string Name { get; set; } = null!;
    }
}
