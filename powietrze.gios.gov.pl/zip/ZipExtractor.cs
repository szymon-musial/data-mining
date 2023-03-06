using System.IO.Compression;

namespace powietrze.gios.gov.pl.zip;

public class ZipExtractor
{
    private readonly string _destinationFolder;

    public ZipExtractor(string destinationFolder)
    {
        _destinationFolder = destinationFolder;
    }

    public static void EnsureDirectoryExistsAndItsClean(string destinationFolder)
    {
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }
        DirectoryInfo di = new(destinationFolder);
        di.Delete(true);
    }

    public IEnumerable<Task> ExtractFilesAsync()
    {
        var filesInDirectory = Directory.GetFiles(_destinationFolder);
        foreach(var file in filesInDirectory)
        {
            yield return ExtractorTask(file);
        }
    }

    public Task ExtractorTask(string file)
    {
        return Task.Run(() =>
        {
            string extractPath = $"{Directory.GetParent(file).Parent.FullName}\\extracted_files\\{Path.GetFileNameWithoutExtension(file)}";

            Console.WriteLine($"Extracting {Path.GetFileName(file)}");
            EnsureDirectoryExistsAndItsClean(extractPath);

            ZipFile.ExtractToDirectory(file, extractPath);

            Console.WriteLine($"Extracted {Path.GetFileName(file)}");
        });
    }

}
