using System.Threading;
using System.Threading.Tasks;
using FancyMessengerApi.Dto;
using FancyMessengerApi.Entities;
using FancyMessengerApi.Repository;
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

        private readonly UserRepository _userRepository;
        
        private readonly AuthService _authService;
        
        public SignUpController(UserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            
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
        public async Task<IActionResult> CheckUsernameAsync(
            [FromBody] string username, CancellationToken ct)
        {
            if (await _userRepository.FindOneAsync(username, ct) is null)
                return Ok();

            return Conflict();
        }

        /// <summary>
        /// Register new user and return first auth-data.
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // TODO
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpAsync(
            [FromBody] UserCredentials credentials, CancellationToken ct)
        {
            if (await _userRepository.FindOneAsync( credentials.Username, ct) != null)
                return Conflict();

            _authService.CreatePasswordHash(
                credentials.Password, out var hash, out var salt // TODO awfull interface
            );
            
            var userId = await _userRepository.InsertOneAsync(
                new UserEntity {
                    Username = credentials.Username,
                    PasswordHash = hash,
                    PasswordSalt = salt
                },  
                ct
            );

            // TODO ugly.
            return Ok(
                new {
                    id = userId,
                    credentials.Username,
                    AccessToken = _authService.CreateUserToken(userId)
                }
            );
        }
    }
}