using Microsoft.Extensions.Configuration;
using rapidapi_wettercom;
using System.Text.Json;

var currentUTCTime = DateTime.UtcNow;

Console.WriteLine(currentUTCTime);

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var configurationRoot = builder.Build();

var connectionString = configurationRoot.GetSection("connectionString").Value;
var apiKey = configurationRoot.GetSection("RapidApiKey").Value;

if (connectionString is null)
{
    connectionString = Environment.GetEnvironmentVariable("connectionString");
}

if (apiKey is null)
{
    apiKey = Environment.GetEnvironmentVariable("RapidApiKey");
}

ArgumentNullException.ThrowIfNull(apiKey);

if (connectionString is not null)
{
    AppDbContext.connectionString = connectionString;
}

//string SampleJson = File.ReadAllText("./rapid-forecast.json");
//RapidApiResponse weatherData = JsonSerializer.Deserialize<RapidApiResponse>(SampleJson, new JsonSerializerOptions
//{
//    PropertyNameCaseInsensitive = true
//});

var rapidForecastService = new RapidForecastService(apiKey);
var weatherData = await rapidForecastService.FetchAsync();

var rapidForecastDbModel = RapidForecastService.ProcessResponse(weatherData, currentUTCTime);

var appDbContext = new AppDbContext();

await appDbContext.RapidForecast.AddRangeAsync(rapidForecastDbModel);
var count = await appDbContext.SaveChangesAsync();

Console.WriteLine($"Added {count} items");
