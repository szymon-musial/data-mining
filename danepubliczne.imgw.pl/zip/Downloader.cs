using System.Net;

namespace danepubliczne.imgw.pl.zip;

public class Downloader
{
    private readonly IEnumerable<string> _urls;
    private readonly string _destinationFolder;

    public Downloader(IEnumerable<string> urls, string destinationFolder)
    {
        _urls = urls;
        _destinationFolder = destinationFolder;
        EnsureDirectoryExists(_destinationFolder);
    }

    public static void EnsureDirectoryExists(string destinationFolder)
    {
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }
    }

    public static Task DownloadZipTask(string url, string destinationFolder, int taskId)
    {
        var destinationFileLocation = Path.Combine(destinationFolder, url.Split('/').Last());

        if(Path.Exists(destinationFileLocation))
        {
            // Skip if exists on disk
            return Task.CompletedTask;
        }


        using (var client = new WebClient())
        {
            Console.WriteLine($"Downloading.. {taskId} {url}");
            return client.DownloadFileTaskAsync(url, destinationFileLocation);
        };
    }

    public IEnumerable<Task> GenerateDownloadTasks()
    {
        var filesInDirectory = Directory.GetFiles(_destinationFolder);

        // expected to contain only year.zip file names
        var onlyFileNamesInDirectory = filesInDirectory.Select(x => Path.GetFileNameWithoutExtension(x));
        //var savedYearsArchive = onlyFileNamesInDirectory.Select(int.Parse);

        // remove downloaded file from download list

        // !! TODO
        var urlsToDownload = _urls.Where(url => !onlyFileNamesInDirectory.Contains(url)).ToArray();

        for (int i = 0; i < urlsToDownload.Length; i++)
        {
            var url = urlsToDownload[i];
            yield return DownloadZipTask(url, _destinationFolder, i);
        }

    }
}
