// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using powietrze.gios.gov.pl.Persistence;
using powietrze.gios.gov.pl.xlsxExtractor;
using powietrze.gios.gov.pl.zip;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var configurationRoot = builder.Build();

var connectionString = configurationRoot.GetSection("connectionString").Value;
if (connectionString is null)
{
    connectionString = Environment.GetEnvironmentVariable("connectionString");
}

if (connectionString is not null )
{
    AppDbContext.connectionString = connectionString;
}

var workingFolder = Environment.GetEnvironmentVariable("workingFolder") ?? Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
Console.WriteLine($"Working directory: {workingFolder}");

Downloader downloader = new (UrlArray.Urls, workingFolder + "/downloaded_zip");
var tasks = downloader.GenerateDownloadTasks();
await Task.WhenAll(tasks.ToArray());


ZipExtractor zipExtractor = new(workingFolder + "/downloaded_zip");
await Task.WhenAll(zipExtractor.ExtractFilesAsync().ToArray());
Console.WriteLine("Extracted");

var appDbContext = new AppDbContext();

XlsxExctractor xlsxExctractor = new(workingFolder, appDbContext);
xlsxExctractor.DbSeed(maxRunningTasks: 30);
