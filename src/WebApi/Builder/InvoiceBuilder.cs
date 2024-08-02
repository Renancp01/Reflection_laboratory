using System.Globalization;
using WebApi.Invoice;

namespace WebApi.Builder;

public class InvoiceBuilder(global::Invoice invoice, InvoiceDto invoiceDto)
{
    public InvoiceBuilder WithCurrentInvoice()
    {
        invoice.CurrentInvoice.Title = $"Fatura de {invoiceDto.DueDate.ToString("MMMM", new DateTimeFormatInfo
        {
            MonthNames = ["janeiro", "Fevereiro", "Março", "Abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro", ""]
        })}";

        invoice.CurrentInvoice.Value = invoiceDto.Amount.ToString("C", new NumberFormatInfo
        {
            CurrencySymbol = "R$",
            CurrencyDecimalSeparator = ",",
            CurrencyGroupSeparator = ".",
            CurrencyDecimalDigits = 2
        });

        return this;
    }

    public InvoiceBuilder WithDueDay()
    {
        invoice.DueDay.Value = invoiceDto.DueDate.ToString("dd/MMM", new DateTimeFormatInfo
        {
            MonthNames =
            [
                "janeiro", "Fevereiro", "Março", "Abril", "maio", "junho", "julho", "agosto", "setembro", "outubro",
                "novembro", "dezembro", ""
            ],
            AbbreviatedMonthGenitiveNames =
                ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez", ""]
        });

        return this;
    }

    public InvoiceBuilder WithCloseDay(string title, string value)
    {
        invoice.CloseDay = new CloseDay
        {
            Title = title,
            Value = value
        };
        return this;
    }

    public InvoiceBuilder WithWarning(string warning)
    {
        invoice.Warning = warning;
        return this;
    }

    public global::Invoice Build()
    {
        return invoice;
    }
}