namespace QRcodeGenerator.Contracts;

public record QRCodeModel(string Image, QRCodeConfiguration Configuration);

public class QRCodeConfiguration
{
    public string? Name { get; set; }
    public int Height { get; set; }
    public string? Url { get; set; }
}
