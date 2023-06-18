using airly_data_fetch;
using Microsoft.Extensions.Configuration;

Console.WriteLine(DateTime.Now);

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var configurationRoot = builder.Build();

var connectionString = configurationRoot.GetSection("connectionString").Value;
var apiKey = configurationRoot.GetSection("apiKey").Value;

if (connectionString is null)
{
    connectionString = Environment.GetEnvironmentVariable("connectionString");
}

if (apiKey is null)
{
    apiKey = Environment.GetEnvironmentVariable("apiKey");
}

ArgumentNullException.ThrowIfNull(apiKey);

if (connectionString is not null)
{
    AppDbContext.connectionString = connectionString;
}

var appDbContext = new AppDbContext();
var arilyApiRepository = new ArilyApiRepository(apiKey);
var appRepository = new AppRepostioty(appDbContext);


foreach (var friendlyStationName in Urls.GetFriendlyStationName())
{
    Console.WriteLine($"Processing ... {friendlyStationName.Value}");
    var request = await arilyApiRepository.GetMeasurementsForInstallationId(friendlyStationName);
    if(request is null)
    {
        continue;
    }
    var responseModel = await arilyApiRepository.ProcessResponse(request);
    appRepository.InsertEntity(responseModel, friendlyStationName);
    Console.WriteLine($"Finished {friendlyStationName.Value}");
}

Console.WriteLine("Done");