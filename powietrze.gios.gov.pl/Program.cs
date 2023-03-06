// See https://aka.ms/new-console-template for more information
using powietrze.gios.gov.pl.zip;


Downloader downloader = new (UrlArray.Urls, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\downloaded_zip");
var tasks = downloader.GenerateDownloadTasks();
await Task.WhenAll(tasks.ToArray());


ZipExtractor zipExtractor = new(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\downloaded_zip");
await Task.WhenAll(zipExtractor.ExtractFilesAsync().ToArray());
Console.WriteLine("Extracted");
