using Microsoft.AspNetCore.Mvc;

namespace Zoobook.Service.Administration.Api
{
    /// <summary>
    /// Administration Home controller endpoint
    /// </summary>
    [ApiController]
    [Route("")]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        ///  Administration Home API home
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return Ok("Zoobook Systems LLC Administration-Home API");
        }
    }
}
