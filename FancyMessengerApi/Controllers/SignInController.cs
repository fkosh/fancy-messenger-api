using System.Threading;
using System.Threading.Tasks;
using FancyMessengerApi.Dto;
using FancyMessengerApi.Repository;
using FancyMessengerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    /// <summary>
    /// All for user authorization purposes.
    /// </summary>
    [AllowAnonymous, Route("api/sign-in")]
    public class SignInController : ApiController
    {
        private readonly UserRepository _userRepository;
        
        private readonly AuthService _authService;
        
        public SignInController(UserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            
            _authService = authService;
        }
        
        /// <summary>
        /// Authenticate user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]   
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SigInAsync(
            [FromBody] UserCredentials credentials, CancellationToken ct)
        {
            var user = await _userRepository.FindOneAsync(credentials.Username, ct);

            if (user is null)
                return Unauthorized();

            var isPasswordValid = _authService.VerifyPasswordHash(
                credentials.Password, user.PasswordHash, user.PasswordSalt
            );
            
            if (!isPasswordValid)
                return Unauthorized();
            
            return Ok(
                new {
                    user.Id,
                    credentials.Username,
                    AccessToken = _authService.CreateUserToken(user.Id)
                }
             );
        }
    }
}