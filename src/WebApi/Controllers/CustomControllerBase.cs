using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
        protected string GetProcessedHeader()
        {
            if (HttpContext.Items.TryGetValue("Teste", out var headerValue))
            {
                return headerValue.ToString();
            }

            throw new Exception("Required header not found.");
        }
    }
}
