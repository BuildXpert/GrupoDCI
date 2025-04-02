using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Build_Xpert.Model
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Cliente"; 
    }
}
