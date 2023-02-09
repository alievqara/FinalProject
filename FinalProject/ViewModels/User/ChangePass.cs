using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FinalProject.ViewModels.User
{
    public class ChangePass
    {
        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password), Display(Name = "Confirm Password"), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
