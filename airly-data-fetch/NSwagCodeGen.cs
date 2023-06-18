using NSwag.CodeGeneration.CSharp;
using NSwag;

namespace airly_data_fetch;

public class NSwagCodeGen
{
    public string Url { get; }
    public string Path { get; }

    public NSwagCodeGen(string url, string path)
    {
        Url = url;
        Path = path;
    }

    async Task GenerateCSharpClient() =>
          await GenerateClient(
              document: await OpenApiDocument.FromUrlAsync(Url),
              generatePath: Path,
              generateCode: (OpenApiDocument document) =>
              {
                  var settings = new CSharpClientGeneratorSettings
                  {
                      UseBaseUrl = false
                  };

                  var generator = new CSharpClientGenerator(document, settings);
                  var code = generator.GenerateFile();
                  return code;
              }
          );


    async Task GenerateClient(OpenApiDocument document, string generatePath, Func<OpenApiDocument, string> generateCode)
    {
        Console.WriteLine($"Generating {generatePath}...");

        var code = generateCode(document);

        await File.WriteAllTextAsync(generatePath, code);
    }

}
