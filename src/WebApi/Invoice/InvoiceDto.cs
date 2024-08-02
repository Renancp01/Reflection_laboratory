namespace WebApi.Invoice;

public class InvoiceDto
{
    public DateTime DueDate { get; set; }
    public DateTime ActualDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public decimal Amount { get; set; }
    public decimal MinPaymentValue { get; set; }
    public decimal Previousbalence { get; set; }
    public string Status { get; set; }
    public bool Current { get; set; }
    public string PaymentCode { get; set; }
    public decimal MinPayableInvoiceAmount { get; set; }
    public int TotalPayments { get; set; }
}