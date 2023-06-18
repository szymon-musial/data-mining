using System.Net.Http.Json;

namespace airly_data_fetch;
public class ArilyApiRepository
{
    readonly HttpClient _client;
    const string MeasurementsForInstallationUrl = "/v2/measurements/installation/";

    public ArilyApiRepository(string apiKey)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://airapi.airly.eu");
        _client.DefaultRequestHeaders.Add("apikey", apiKey);
    }


    public async Task<HttpContent> GetMeasurementsForInstallationId(KeyValuePair<int, string> friendlyStationName)
    {
        var response = await _client.GetAsync($"{MeasurementsForInstallationUrl}/?installationId={friendlyStationName.Key}&indexType=AIRLY_CAQI&standardType=WHO&includeWind=true");

        if(!response.IsSuccessStatusCode)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Response was not success HTTP ({response.StatusCode}) for station id: {friendlyStationName.Key} - {friendlyStationName.Value}");
            Console.ResetColor();
            return null;
        }

        return response.Content;        
    }

    public async Task<Measurements> ProcessResponse(HttpContent httpContent) => await httpContent.ReadFromJsonAsync<Measurements>();
}
