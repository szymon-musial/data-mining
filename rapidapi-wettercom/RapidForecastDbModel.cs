using System.ComponentModel.DataAnnotations.Schema;

namespace rapidapi_wettercom
{
    [Table("RapidForecast")]
    public class RapidForecastDbModel
    {
        // PK
        public int Id { get; set; }
        public DateTime RequestTime { get; set; }


        // public WeatherInfo Weather { get; set; }
        public int Weather_State { get; set; }
        public string Weather_Text { get; set; }
        public string Weather_Icon { get; set; }


        //public Precipitation Prec { get; set; }
        public double Prec_Sum { get; set; }
        public double Prec_Probability { get; set; }
        public int Prec_Class { get; set; }


        //public TemperatureInfo Temperature { get; set; }
        public int Temperature_Avg { get; set; }

        //public CloudsInfo Clouds { get; set; }
        public int Clouds_High { get; set; }
        public int Clouds_Low { get; set; }
        public int Clouds_Middle { get; set; }


        //public WindInfo Wind { get; set; }
        public string Wind_Unit { get; set; }
        public string Wind_Direction { get; set; }
        public string Wind_Text { get; set; }
        public int Wind_Avg { get; set; }
        public bool Wind_SignificationWind { get; set; }


        //public WindchillInfo Windchill { get; set; }
        public int Windchill_Avg { get; set; }


        // Original resp (WeatherData class)
        public DateTime Date { get; set; }
        public int Period { get; set; }
        public int? FreshSnow { get; set; }
        public int? SunHours { get; set; }
        public int? RainHours { get; set; }
        public int Pressure { get; set; }
        public int RelativeHumidity { get; set; }
        public bool IsNight { get; set; }
    }

}
