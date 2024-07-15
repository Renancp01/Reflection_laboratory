namespace WebApi.Response;

public enum InvoiceResponseCode
{
    InvoiceNotFound = 1001,
    InvalidInvoice = 1002
}

public enum LimitResponseCode
{
    LimitExceeded = 2001,
    InvalidLimit = 2002
}

public class ApiResponse<T>
{
    private static readonly Dictionary<InvoiceResponseCode, string> InvoiceMessages = new Dictionary<InvoiceResponseCode, string>
    {
        { InvoiceResponseCode.InvoiceNotFound, "Invoice not found" },
        { InvoiceResponseCode.InvalidInvoice, "Invalid invoice" }
    };

    private static readonly Dictionary<LimitResponseCode, string> LimitMessages = new Dictionary<LimitResponseCode, string>
    {
        { LimitResponseCode.LimitExceeded, "Limit exceeded" },
        { LimitResponseCode.InvalidLimit, "Invalid limit" }
    };
    public T Data { get; set; }
    public List<ErrorDetails> ErrorsDetails { get; set; }
    public string HumanReadableMessage { get; set; }
    public static ApiResponse<T> SuccessResponse(T data)
    {
        return new ApiResponse<T> { Data = data };
    }
    public static ApiResponse<T> ErrorResponse<TCode>(List<(TCode code, string customMessage)> codes, string humanReadableMessage)
    {
        var errors = new List<ErrorDetails>();
        foreach (var (code, customMessage) in codes)
        {
            var defaultMessage = code switch
            {
                InvoiceResponseCode invoiceCode => InvoiceMessages.GetValueOrDefault(invoiceCode, "Unknown error"),
                LimitResponseCode limitCode => LimitMessages.TryGetValue(limitCode, out var limitMessage) ? limitMessage : "Unknown error",
                _ => "Unknown error"
            };

            errors.Add(new ErrorDetails
            {
                Code = 404,
                Message = customMessage ?? defaultMessage
            });
        }

        return new ApiResponse<T> { ErrorsDetails = errors, HumanReadableMessage = humanReadableMessage };
    }


    public static ApiResponse<T> ErrorResponse<TCode>(TCode code, string customMessage, string humanReadableMessage)
    {
        var defaultMessage = code switch
        {
            InvoiceResponseCode invoiceCode => InvoiceMessages.GetValueOrDefault(invoiceCode, "Unknown error"),
            LimitResponseCode limitCode => LimitMessages.GetValueOrDefault(limitCode, "Unknown error"),
            _ => "Unknown error"
        };

        var errors = new List<ErrorDetails>
        {
            new ErrorDetails()
            {
                Code = 404,
                Message = customMessage ?? defaultMessage
            }
        };

        return new ApiResponse<T> { ErrorsDetails = errors, HumanReadableMessage = humanReadableMessage };
    }
}

public class ErrorDetails
{
    public int Code { get; set; }
    public string Message { get; set; }
}
