// See https://aka.ms/new-console-template for more information
using powietrze.gios.gov.pl.zip;


var downloader = new Downloader(UrlArray.Urls, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\downloaded_zip");
var tasks = downloader.GenerateDownloadTasks();
await Task.WhenAll(tasks.ToArray());
Console.WriteLine("Downloaded");

