using Microsoft.AspNetCore.Mvc;

namespace Shared.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}