using danepubliczne.imgw.pl.csv_extractor;
using danepubliczne.imgw.pl.Persistence;
using danepubliczne.imgw.pl.zip;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var configurationRoot = builder.Build();

var connectionString = configurationRoot.GetSection("connectionString").Value;
if (connectionString is null)
{
    connectionString = Environment.GetEnvironmentVariable("connectionString");
}

if (connectionString is not null)
{
    AppDbContext.connectionString = connectionString;
}

var workingFolder = Environment.GetEnvironmentVariable("workingFolder") ?? Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
Console.WriteLine($"Working directory: {workingFolder}");

Downloader downloader = new(new UrlArray().Urls, workingFolder + "/downloaded_zip");
var tasks = downloader.GenerateDownloadTasks();
await Task.WhenAll(tasks.ToArray());

ZipExtractor zipExtractor = new(workingFolder + "/downloaded_zip");
await Task.WhenAll(zipExtractor.ExtractFilesAsync().ToArray());
Console.WriteLine("Extracted");

var appDbContext = new AppDbContext();


CsvExtractor csvExtractor = new(workingFolder, appDbContext);
csvExtractor.DbSeed();
