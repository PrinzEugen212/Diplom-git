using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [Route("/")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<String>> Ping()
        {
            return "pong json";
        }
    }
}
