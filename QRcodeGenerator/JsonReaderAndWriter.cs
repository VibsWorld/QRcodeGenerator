using System.Text.Json;

namespace QRcodeGenerator
{
    public class JsonReaderAndWriter
    {
        public static string QrCodeConfigurationFile = "QRCodeConfiguration.json";
        private readonly string dataDirectoryPath;
        private static readonly object lockChecker = new();

        public JsonReaderAndWriter(string? dataDirectory = null)
        {
            dataDirectoryPath = Path.Combine(
                Environment.CurrentDirectory,
                dataDirectory ?? "AppData"
            );

            if (!Directory.Exists(dataDirectoryPath))
                Directory.CreateDirectory(dataDirectoryPath!);
        }

        public T ReadFileFromDirectory<T>(string fileName)
        {
            lock (lockChecker)
            {
                return JsonSerializer.Deserialize<T>(
                        File.ReadAllText(Path.Combine(dataDirectoryPath, fileName))
                    )
                    ?? throw new NullReferenceException(
                        "unable to deserialize the file. Please check if file exists and is in the correct json format"
                    );
            }
        }

        public void WriteFileToDirectory<T>(T obj, string fileName)
        {
            lock (lockChecker)
                File.WriteAllText(
                    Path.Combine(dataDirectoryPath, fileName),
                    JsonSerializer.Serialize(
                        obj,
                        options: new JsonSerializerOptions { WriteIndented = true }
                    )
                );
        }
    }
}
