using Contracts;
using Contracts.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCController(IConfiguration configuration, CardService cardService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("AAA")]
        public async Task<IActionResult> Get([FromServices] IOptions<Settings> settings)
        {

            var card = await cardService.GetCardAsync();
            //var a = settings.Value;
            //var c = new Card();
            return Ok(card);
        }

        [HttpGet(Name = "LALALA")]
        [ServiceFilter(typeof(SpecificHeadersFilter))]
        //[ServiceFilter(typeof(SpecificHeadersFilter))]
        public async Task<IActionResult> LALALA()
        {
            var card = await cardService.GetCardAsync();
            //var a = settings.Value;
            //var c = new Card();
            return Ok(card);
        }
    }
}