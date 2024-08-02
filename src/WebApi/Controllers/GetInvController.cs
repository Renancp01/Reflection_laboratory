using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Builder;
using WebApi.Invoice;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GetInvController(IConfiguration configuration) : Controller
{
    // GET
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("TesteDic")]
    public async Task<IActionResult> TesteDic([FromServices] IOptions<InvoiceSettings> invoicess)
    {
        var invoiceDto = new InvoiceDto
        {
            DueDate = new DateTime(2024, 09, 5),
            ActualDate = new DateTime(2024, 09, 02),
            ClosingDate = new DateTime(2024, 08, 25),
            Amount = 132032.00M,
            MinPaymentValue = 1000.00M,
            Previousbalence = 1000.00M,
            PaymentCode = string.Empty,
        };

        // var invoices = GetDic();
        invoicess.Value.ConfigInvoice.TryGetValue("open", out var invoice);

        var builder = new InvoiceBuilder(invoice, invoiceDto)
            .WithCurrentInvoice()
            .WithDueDay()
            .WithCloseDay("Vencimento", "29/jun")
            .WithWarning("Evite o atraso da sua fatura, ele gera juros")
            .Build();


        return Ok(builder);
    }

    [NonAction]
    public Dictionary<string, global::Invoice> GetDic()
    {
        var openInvoice = new global::Invoice
        {
            CurrentInvoice = new CurrentInvoice
            {
                Title = "Fatura de maio",
                Value = "R$ 132.032,00",
                Status = new InvoiceStatus
                {
                    Text = "Aberta",
                    Type = "Info"
                }
            },
            DueDay = new DueDay
            {
                Icon = new Icon
                {
                    Type = "Icon",
                    Value = "Calendar"
                },
                Title = "Vencimento",
                Value = "05/jun"
            },
            CloseDay = new CloseDay
            {
                Icon = new Icon
                {
                    Type = "ICON",
                    Value = "gift"
                },
                Title = "Vencimento",
                Value = "29/jun"
            },
            Warning = "Evite o atraso da sua fatura, ele gera juros",
            Buttons = new List<Button1>
            {
                new Button1
                {
                    Type = "deeplink",
                    Text = "Adicionar a carteira",
                    Url = "/card/add",
                    Params = new Dictionary<string, string>()
                },
                new Button1
                {
                    Type = "deeplink",
                    Text = "Adicionar a carteira",
                    Url = "/card/add",
                    Params = new Dictionary<string, string>()
                }
            }
        };
        var invoices = new Dictionary<string, global::Invoice>
        {
            { "open", openInvoice },
            // { "closed", closedInvoice }
        };

        return invoices;
    }
}