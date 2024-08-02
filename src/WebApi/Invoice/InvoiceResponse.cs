public class InvoiceStatus
{
    public string Text { get; set; }
    public string Type { get; set; }
}

public class Icon
{
    public string Type { get; set; }
    public string Value { get; set; }
}

public class DueDay
{
    public Icon Icon { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}

public class CloseDay
{
    public Icon Icon { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}

public class Button1
{
    public string Type { get; set; }
    public string Text { get; set; }
    public string Url { get; set; }
    public Dictionary<string, string> Params { get; set; }
}

public class CurrentInvoice
{
    public string Title { get; set; }
    public string Value { get; set; }
    public InvoiceStatus Status { get; set; }
}

public class Invoice
{
    public CurrentInvoice CurrentInvoice { get; set; }
    public DueDay DueDay { get; set; }
    public CloseDay CloseDay { get; set; }
    public string Warning { get; set; }
    public List<Button1> Buttons { get; set; }
}

