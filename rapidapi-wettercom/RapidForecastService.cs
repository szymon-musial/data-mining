using System.Net.Http.Json;

namespace rapidapi_wettercom;

public class RapidForecastService
{
    public readonly string _rapidApiKey;
    readonly HttpClient _client;

    public RapidForecastService(string rapidApiKey)
    {
        _rapidApiKey = rapidApiKey;
    }

    public async Task<RapidApiResponse> FetchAsync()
    {
        //var response = await _client.GetAsync("/rapidapi/forecast/Krakow/hourly");

        //return await response.Content.ReadFromJsonAsync<RapidApiResponse>();

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://forecast9.p.rapidapi.com/rapidapi/forecast/Krakow/hourly/"),
            Headers =
            {
                { "X-RapidAPI-Key", _rapidApiKey },
                { "X-RapidAPI-Host", "forecast9.p.rapidapi.com" },
            },
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RapidApiResponse>();
    }



    public static IEnumerable<RapidForecastDbModel> ProcessResponse(RapidApiResponse rapidApiResponse, DateTime timeNow)
        // todo fixed datetime
        => rapidApiResponse.Forecast.Items.Select(resp => new RapidForecastDbModel()
        {
            RequestTime = timeNow,

            Weather_State = resp.Weather.State,
            Weather_Text = resp.Weather.Text,
            Weather_Icon = resp.Weather.Icon,

            Prec_Sum = resp.Prec.Sum,
            Prec_Probability = resp.Prec.Probability,
            Prec_Class = resp.Prec.Class,

            Temperature_Avg = resp.Temperature.Avg,

            Clouds_High = resp.Clouds.High,
            Clouds_Low = resp.Clouds.Low,
            Clouds_Middle = resp.Clouds.Middle,

            Wind_Unit = resp.Wind.Unit,
            Wind_Direction = resp.Wind.Direction,
            Wind_Text = resp.Wind.Text,
            Wind_Avg = resp.Wind.Avg,
            Wind_SignificationWind = resp.Wind.SignificationWind,

            Windchill_Avg = resp.Windchill.Avg,


            // Original resp (WeatherData class)
            Date = resp.Date,
            Period = resp.Period,
            FreshSnow = resp.FreshSnow,
            //Weather = resp.Weather,
            SunHours = resp.SunHours,
            RainHours = resp.RainHours,
            //Prec = resp.Prec,
            //Temperature = resp.Temperature,
            Pressure = resp.Pressure,
            RelativeHumidity = resp.RelativeHumidity,
            //Clouds = resp.Clouds,
            //Wind = resp.Wind,
            //Windchill = resp.Windchill,
            IsNight = resp.IsNight,
        });
}
