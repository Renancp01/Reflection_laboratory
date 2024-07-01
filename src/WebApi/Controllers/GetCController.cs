using System.Text.Json;
using System.Text.Json.Serialization;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public GetCController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet()]
        public IActionResult Get([FromServices] IOptions<Settings> settings)
        {
            var a = settings.Value;
            
            return Ok();
        }
    }
}