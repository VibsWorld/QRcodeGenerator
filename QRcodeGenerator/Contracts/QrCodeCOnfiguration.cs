namespace QRcodeGenerator.Contracts;

public record QRCodeModel(Dictionary<string, string> QRCodes, int height = 250);

public class QRCodeConfiguration
{
    public string? Name { get; set; }
    public int Height { get; set; }
    public string? Url { get; set; }
}
