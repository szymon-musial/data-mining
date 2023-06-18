using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace powietrze.gios.gov.pl.Entities;

public class CSVEntity
{
    /// <summary>
    /// Kod stacji      
    /// </summary>
    [Index(0)]
    public int StationCode { get; set; }

    /// <summary>
    /// Nazwa stacji 
    /// </summary>
    [Index(1)]
    public string StationName { get; set; }

    /// <summary>
    /// Rok                                                                     
    /// </summary>
    [Index(2)]
    public int Year { get; set; }

    /// <summary>
    /// Miesiąc
    /// </summary>
    [Index(3)]
    public int Month { get; set; }

    /// <summary>
    /// Dzień                                                                   
    /// </summary>
    [Index(4)]
    public int Day { get; set; }

    /// <summary>
    /// Godzina                                                                 
    /// </summary>
    [Index(5)]
    public int Hour { get; set; }

    /// <summary>
    /// Temperatura powietrza [°C] 
    /// </summary>
    [Index(6)]
    public double Temperature { get; set; }

    /// <summary>
    /// Status pomiaru TEMP     
    /// </summary>
    [Index(7)]
    public string TemperatureMeasurementStatus { get; set; }

    /// <summary>
    /// Temperatura termometru zwilżonego [°C]  
    /// </summary>
    [Index(8)]
    public double WetBulbTemperature { get; set; }

    /// <summary>
    /// Status pomiaru TTZW  
    /// </summary>
    [Index(9)]
    public string WetBulbTemperatureMeasurementStatus { get; set; }

    /// <summary>
    /// Wskaźnik lodu [L/W]       
    /// </summary>
    [Index(10)]
    public string IceIndicator { get; set; }

    /// <summary>
    /// Wskaźnik wentylacji [W/N]     
    /// </summary>
    [Index(11)]
    public string VentilationIndicator { get; set; }

    /// <summary>
    /// Wilgotność względna [%]    
    /// </summary>
    [Index(12)]
    public int RelativeHumidity { get; set; }

    /// <summary>
    /// Status pomiaru WLGW    
    /// </summary>
    [Index(13)]
    public string RelativeHumidityMeasurementStatus { get; set; }

    /// <summary>
    /// Kod kierunku wiatru [kod]   
    /// </summary>
    [Index(14)]
    public string WindDirectionCode { get; set; }

    /// <summary>
    /// Status pomiaru DKDK   
    /// </summary>
    [Index(15)]
    public string WindDirectionMeasurementStatus { get; set; }

    /// <summary>
    /// Prędkość wiatru [m/s]
    /// </summary>
    [Index(16)]
    public double WindSpeed { get; set; }

    /// <summary>
    /// Status pomiaru FWR 
    /// </summary>
    [Index(17)]
    public string WindSpeedMeasurementStatus { get; set; }

    /// <summary>
    /// Zachmurzenie ogólne 
    /// </summary>
    [Index(18)]
    public int CloudCover { get; set; }

    /// <summary>
    /// Status pomiaru ZOGK     
    /// </summary>
    [Index(19)]
    public string CloudCoverMeasurementStatus { get; set; }

    /// <summary>
    /// Widzialność [kod]   
    /// </summary>
    [Index(20)]
    public string VisibilityCode { get; set; }

    /// <summary>
    /// Status pomiaru WID 
    /// </summary>
    [Index(21)]
    public string VisibilityMeasurementStatus { get; set; }
}




