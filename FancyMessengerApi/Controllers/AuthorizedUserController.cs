using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    [Authorize]
    public abstract class AuthorizedUserController : ControllerBase
    {
        protected string AuthorizedUserId => HttpContext?.User?.FindFirst(
            // It is "sub" field in jwt-token...
            ClaimTypes.NameIdentifier
        )?.Value;
    }
}