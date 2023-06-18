using System.ComponentModel.DataAnnotations;

namespace danepubliczne.imgw.pl.Entities;

public class WatherDataEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    /// <summary>
    /// Kod stacji      
    /// </summary>
    public int StationCode { get; set; }

    /// <summary>
    /// Nazwa stacji 
    /// </summary>
    public string StationName { get; set; }

    /// <summary>
    /// Temperatura powietrza [°C] 
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Status pomiaru TEMP     
    /// </summary>
    public string? TemperatureMeasurementStatus { get; set; }

    /// <summary>
    /// Temperatura termometru zwilżonego [°C]  
    /// </summary>
    public double? WetBulbTemperature { get; set; }

    /// <summary>
    /// Status pomiaru TTZW  
    /// </summary>
    public string? WetBulbTemperatureMeasurementStatus { get; set; }

    /// <summary>
    /// Wskaźnik lodu [L/W]       
    /// </summary>
    public string? IceIndicator { get; set; }

    /// <summary>
    /// Wskaźnik wentylacji [W/N]     
    /// </summary>
    public string? VentilationIndicator { get; set; }

    /// <summary>
    /// Wilgotność względna [%]    
    /// </summary>
    public int? RelativeHumidity { get; set; }

    /// <summary>
    /// Status pomiaru WLGW    
    /// </summary>
    public string? RelativeHumidityMeasurementStatus { get; set; }

    /// <summary>
    /// Kod kierunku wiatru [kod]   
    /// </summary>
    public string? WindDirectionCode { get; set; }

    /// <summary>
    /// Status pomiaru DKDK   
    /// </summary>
    public string? WindDirectionMeasurementStatus { get; set; }

    /// <summary>
    /// Prędkość wiatru [m/s]
    /// </summary>
    public double? WindSpeed { get; set; }

    /// <summary>
    /// Status pomiaru FWR 
    /// </summary>
    public string? WindSpeedMeasurementStatus { get; set; }

    /// <summary>
    /// Zachmurzenie ogólne 
    /// </summary>
    public int? CloudCover { get; set; }

    /// <summary>
    /// Status pomiaru ZOGK     
    /// </summary>
    public string? CloudCoverMeasurementStatus { get; set; }

    /// <summary>
    /// Widzialność [kod]   
    /// </summary>
    public string? VisibilityCode { get; set; }

    /// <summary>
    /// Status pomiaru WID 
    /// </summary>
    public string? VisibilityMeasurementStatus { get; set; }

}
