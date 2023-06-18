using System.IO.Compression;

namespace danepubliczne.imgw.pl.zip;

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

        string extractPath = Path.Combine(Directory.GetParent(_destinationFolder).FullName, "extracted_files");
        EnsureDirectoryExistsAndItsClean(extractPath);

        foreach (var file in filesInDirectory)
        {
            yield return ExtractorTask(file, extractPath);
        }
    }

    public Task ExtractorTask(string file, string extractPath)
    {
        return Task.Run(() =>
        {
            try
            {
                Console.WriteLine($"Extracting {Path.GetFileName(file)}");
                ZipFile.ExtractToDirectory(file, extractPath);

            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error with file {file}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine($"Extracted {Path.GetFileName(file)}");
            }
        });
    }

}
