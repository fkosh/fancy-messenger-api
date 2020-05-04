using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    [ApiController, Produces("application/json")]
    public abstract class ApiController : ControllerBase { }
}