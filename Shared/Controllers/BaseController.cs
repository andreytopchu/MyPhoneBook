using Microsoft.AspNetCore.Mvc;

namespace Shared.Bll.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}