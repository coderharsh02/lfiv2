using lfiApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace lfiApi.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
