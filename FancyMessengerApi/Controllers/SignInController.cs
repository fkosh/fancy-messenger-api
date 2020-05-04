using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    /// <summary>
    /// All for user authorization purposes.
    /// </summary>
    [AllowAnonymous, Route("api/sign-in")]
    public class SignInController : ApiController
    {
        /// <summary>
        /// Authenticate user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.Unauthorized)]   
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SigInAsync([FromBody] object signInData)
        { 
            return Ok();
        }
    }
}