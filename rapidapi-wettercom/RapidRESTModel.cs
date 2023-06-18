namespace rapidapi_wettercom;

public class RapidApiResponse
{
    //public object Location { get; set; }
    public Forecast Forecast { get; set; }
}

public class Forecast
{
    public IEnumerable<WeatherData> Items { get; set; }
}


public class WeatherData
{
    public DateTime Date { get; set; }
    public int Period { get; set; }
    public int? FreshSnow { get; set; }
    public WeatherInfo Weather { get; set; }
    public int? SunHours { get; set; }
    public int? RainHours { get; set; }
    public Precipitation Prec { get; set; }
    public TemperatureInfo Temperature { get; set; }
    public int Pressure { get; set; }
    public int RelativeHumidity { get; set; }
    public CloudsInfo Clouds { get; set; }
    public WindInfo Wind { get; set; }
    public WindchillInfo Windchill { get; set; }
    //public SnowLineInfo SnowLine { get; set; }
    public bool IsNight { get; set; }
}

public class WeatherInfo
{
    public int State { get; set; }
    public string Text { get; set; }
    public string Icon { get; set; }
}

public class Precipitation
{
    public double Sum { get; set; }
    public double Probability { get; set; }
    
    // allways null
    //public object SumAsRain { get; set; }
    public int Class { get; set; }
}

public class TemperatureInfo
{
    public int Avg { get; set; }
}

public class CloudsInfo
{
    public int High { get; set; }
    public int Low { get; set; }
    public int Middle { get; set; }
}

public class WindInfo
{
    public string Unit { get; set; }
    public string Direction { get; set; }
    public string Text { get; set; }
    public int Avg { get; set; }

    // allways null
    //public object Min { get; set; }
    //public object Max { get; set; }
    //public WindGustsInfo Gusts { get; set; }
    public bool SignificationWind { get; set; }
}

public class WindGustsInfo
{
    public int Value { get; set; }
    public object Text { get; set; }
}

public class WindchillInfo
{
    public int Avg { get; set; }

    // allways null
    //public object Min { get; set; }
    //public object Max { get; set; }
}

// not needed
//public class SnowLineInfo
//{
//    public object Avg { get; set; }
//    public object Min { get; set; }
//    public object Max { get; set; }
//    public string Unit { get; set; }
//}
