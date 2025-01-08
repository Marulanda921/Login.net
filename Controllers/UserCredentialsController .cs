using ApiLoginFull.Model;
using ApiLoginFull.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLoginFull.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserCredentialsController : ControllerBase
    {
        private readonly UserCredentialsService _userCredentialsService;

        public UserCredentialsController(UserCredentialsService userCredentialsService) { 
            _userCredentialsService = userCredentialsService;
        }

        /// <summary>
        /// Creacion de usuarios
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCredentials user) {
            if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password)) {
                return BadRequest("Datos invalidos");
            }

            if (string.IsNullOrWhiteSpace(user.Email) || !IsValidEmail(user.Email) )
            {
                return BadRequest("El correo electronico proporcionado no es valido");
            }

            await _userCredentialsService.CreateUserAsync(user);
            return Ok("Usuario creado con exito");
        }

        /// <summary>
        /// Funcion para validar el email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var regex = new System.Text.RegularExpressions.Regex(emailPattern);
            return regex.IsMatch(email);
        }
    }
}
