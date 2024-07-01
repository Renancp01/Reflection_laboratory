using Contracts.Enums;

namespace Contracts;

public record Link
{
    public LinkType Type { get; set; }
   
    public string Text { get; set; }
    
    public string Url { get; set; }
    
    public Dictionary<string, object> Params { get; set; } = new();
}