using ApiLoginFull.Model;
using ApiLoginFull.Services;
using ApiLoginFull.Utils.Dto;
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
        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidEmail(request.Email)) {
                return BadRequest("Datos Invalidos");
            }

            try
            {
                var user = new UserCredentials
                {
                    Email = request.Email,
                    Password = request.Password
                };

                await _userCredentialsService.CreateUserAsync(user);
                return Ok(new { message = "Usuario creado con éxito" });
              
            }catch (Exception ex)
            {
                return StatusCode(500, "Error al crear el usuario: " + ex.Message);
            }

        }

        /// <summary>
        /// Logear usuario
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            try
            {
                var response = await _userCredentialsService.LoginAsync(request.Email, request.Password);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized("Credenciales Invalidas");
            }
            catch (Exception ex) {
                return StatusCode(500, "Error en el login" + ex.Message);
            }
        
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
