using System.ComponentModel.DataAnnotations;

namespace Build_Xpert.Model
{
    public class RegisterUser
    {
        [Required]
        [Display(Name = "Nombre Completo")]
        public string UserName { get; set; } = "";

        [Required]
        [Phone]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; } = "";

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
