/*
 Credits code-maze https://code-maze.com/aspnetcore-generate-qr-codes-with-qrcoder/?unapproved=62125&moderation-hash=e025997d320119429505d3f67289632d#comment-62125
 */

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QRcodeGenerator;
using QRcodeGenerator.Contracts;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace QRcodeGenerator.Controllers;

[ApiController]
//[Route("[controller]")]
public class QrConfigurationController : ControllerBase
{
    private readonly ILogger<QrConfigurationController> logger;

    private readonly JsonReaderAndWriter jsonReaderAndWriter;
    private readonly QRCodeGenerator qrGenerator = new();

    public QrConfigurationController(
        ILogger<QrConfigurationController> logger,
        JsonReaderAndWriter jsonReaderAndWriter
    )
    {
        this.logger = logger;
        this.jsonReaderAndWriter = jsonReaderAndWriter;
    }

    [HttpGet]
    [Route("/v1/GetQRCode")]
    public async Task<IActionResult> GetQRCode()
    {
        await Task.Delay(1);
        QRCodeConfiguration qrConfiguration = ReadQrCodeConfirgurationFromFile();
        logger.LogInformation("Code is now read from Qr Code");
        var model = new QRCodeModel(GenerateQRCodeURL(qrConfiguration.Url!), qrConfiguration);
        return Ok(model);
    }

    [HttpPost]
    [Route("/v1/UpdateQrCodeConfigurationFile")]
    public async Task<IActionResult> UpdateQrCodeConfigurationFile(
        [FromBody, Required] QRCodeConfiguration qRCodeConfiguration
    )
    {
        await Task.Delay(1);
        if (qRCodeConfiguration is null)
            return ValidationProblem("Body is empty");

        if (
            qRCodeConfiguration.Height == 0
            || string.IsNullOrWhiteSpace(qRCodeConfiguration.Name)
            || string.IsNullOrWhiteSpace(qRCodeConfiguration.Url)
        )
            return ValidationProblem("Invalid QR name or height");

        jsonReaderAndWriter.WriteFileToDirectory(
            qRCodeConfiguration,
            JsonReaderAndWriter.QrCodeConfigurationFile
        );

        return Ok("success");
    }

    private string GenerateQRCodeURL(string url)
    {
        var qrCodeData = qrGenerator.CreateQrCode(new Url(url));

        return GeneratePng(qrCodeData);
    }

    private static string GeneratePng(QRCodeData data)
    {
        using var qrCode = new PngByteQRCode(data);
        var qrCodeImage = qrCode.GetGraphic(20);
        return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
    }

    private QRCodeConfiguration ReadQrCodeConfirgurationFromFile()
    {
        return jsonReaderAndWriter.ReadFileFromDirectory<QRCodeConfiguration>(
            JsonReaderAndWriter.QrCodeConfigurationFile
        );
    }
}
