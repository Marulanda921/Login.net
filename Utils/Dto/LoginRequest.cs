using System.ComponentModel.DataAnnotations;

namespace ApiLoginFull.Utils.Dto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
