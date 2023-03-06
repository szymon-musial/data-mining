using System.Net;

namespace powietrze.gios.gov.pl.zip;

public class Downloader
{
    private readonly IDictionary<int, string> _urls;
    private readonly string _destinationFolder;

    public Downloader(IDictionary<int, string> urls, string destinationFolder)
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

    public static Task DownloadZipTask(string url, string destinationFolder, int year)
    {
        var destinationFileLocation = $"{destinationFolder}\\{year}.zip";
        using (var client = new WebClient())
        {
            int lastPercentage = 0;

            client.DownloadProgressChanged += (s, e) =>
            {
                var currentPercentage = (int)(((float)e.BytesReceived) / ((float)e.TotalBytesToReceive) * 10);

                if (lastPercentage != currentPercentage) {

                    lastPercentage = currentPercentage;
                    Console.WriteLine($"{year}: {string.Join(' ', Enumerable.Range(0, currentPercentage).Select(x => (x+1).ToString() + "0" )) }");
                }
            };

            Console.WriteLine($"Downloading.. {year}");
            return client.DownloadFileTaskAsync(url, destinationFileLocation);
        };
    }

    public IEnumerable<Task> GenerateDownloadTasks()
    {
        var filesInDirectory = Directory.GetFiles(_destinationFolder);

        // expected to contain only year.zip file names
        var onlyFileNamesInDirectory = filesInDirectory.Select(x => Path.GetFileNameWithoutExtension(x));
        var savedYearsArchive = onlyFileNamesInDirectory.Select(int.Parse);

        // remove downloaded file from download list
        var urlsToDownload = _urls.Where(url => !savedYearsArchive.Contains(url.Key));

        foreach (var url in urlsToDownload)
        {
            yield return DownloadZipTask(url.Value, _destinationFolder, url.Key);
        }
    }
}
