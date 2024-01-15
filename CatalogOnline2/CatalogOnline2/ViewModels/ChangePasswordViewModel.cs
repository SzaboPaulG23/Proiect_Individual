using System.ComponentModel.DataAnnotations;

namespace CatalogOnline2.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
