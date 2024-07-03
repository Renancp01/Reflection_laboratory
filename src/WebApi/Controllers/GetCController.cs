using System.Text.Json;
using System.Text.Json.Serialization;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCController : ControllerBase
    {
        private readonly CardService _cardService;
        private readonly IConfiguration _configuration;

        public GetCController(IConfiguration configuration, CardService cardService)
        {
            _configuration = configuration;
            _cardService = cardService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromServices] IOptions<Settings> settings)
        {

            var card = await _cardService.GetCardAsync();
            //var a = settings.Value;
            //var c = new Card();
            return Ok(card);
        }
    }
}