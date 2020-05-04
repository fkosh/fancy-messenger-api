using System.Threading.Tasks;
using FancyMessengerApi.Dto;
using FancyMessengerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    /// <summary>
    /// All for user registration purposes.
    /// </summary>
    [AllowAnonymous, Route("api/sign-up")]
    public class SignUpController : ApiController
    {
        // TODO
        // private readonly ILogger<WeatherForecastController> _logger;
        //
        // public WeatherForecastController(ILogger<WeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }

        private readonly AuthService _authService;
        
        public SignUpController(AuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Check is username free for register.
        /// </summary>
        /// <returns>
        /// 200 - free for register.
        /// 409 - busy for register.
        /// </returns>
        [HttpPost("checks/username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CheckUsernameAsync([FromBody] string username)
        {
            // TODO

            return Ok();
        }

        /// <summary>
        /// Register new user and return first auth-data.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // TODO
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpAsync([FromBody] UserCredentials credentials)
        {
            var token = _authService.CreateUserToken("123");

            return Ok(token);
        }
    }
}