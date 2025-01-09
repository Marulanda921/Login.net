using System.ComponentModel.DataAnnotations;

namespace ApiLoginFull.Utils.Dto
{
    public class RegisterRequest
    {

        [Required(ErrorMessage = " El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
