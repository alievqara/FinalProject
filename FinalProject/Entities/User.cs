using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Surname { get; set; }

        [Required]
        [StringLength(255)]
        public string FatherName { get; set; }

        public string? Image { get; set; }

        [Required]
        [DataType("Money")]
        public decimal Wage { get; set; }

        [Required]
        public string CreatorID { get; set; }

    }
}
