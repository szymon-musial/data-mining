using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airly_data_fetch;


public enum AveragedValueTimeTypeEnum
{
    Current = 0,
    Forecast = 1,
    History = 2,
}

[Table("Measurements")]
public class MeasurementEntity
{
    [Key]
    public int MeasurementId { get; set; }


    public DateTime RequestDate { get; set; }

    public TimeSpan RequestTime { get; set; }

    public int StationId { get; set; }

    public string StationName { get; set; }

    public List<AveragedValueEntity> AveragedValues { get; set; } = new List<AveragedValueEntity>();


    [NotMapped]
    public AveragedValueEntity Current
    {
        get => AveragedValues.FirstOrDefault(t => t.AveragedValueTimeType == AveragedValueTimeTypeEnum.Current); 
        set
        {
            value.AveragedValueTimeType = AveragedValueTimeTypeEnum.Current;
            AveragedValues.Add(value);
        }
    }

    [NotMapped]
    public IEnumerable<AveragedValueEntity> Forecast
    {
        get => AveragedValues.Where(t => t.AveragedValueTimeType == AveragedValueTimeTypeEnum.Forecast);
        set
        {
            AveragedValues.AddRange(value.Select(i =>
            {
                i.AveragedValueTimeType = AveragedValueTimeTypeEnum.Forecast;
                return i;
            }) );
        }
    }

    [NotMapped]
    public IEnumerable<AveragedValueEntity> History
    {
        get => AveragedValues.Where(t => t.AveragedValueTimeType == AveragedValueTimeTypeEnum.History);
        set
        {
            AveragedValues.AddRange(value.Select(i =>
            {
                i.AveragedValueTimeType = AveragedValueTimeTypeEnum.History;
                return i;
            }));
        }
    }



}

[Table("AveragedValues")]
public class AveragedValueEntity
{
    [Key]
    public int AveragedValuesId { get; set; }

    public int MeasurementEntityFk { get; set; }

    [ForeignKey(nameof(MeasurementEntityFk))]
    public MeasurementEntity MeasurementEntity { get; set; }

    public AveragedValueTimeTypeEnum AveragedValueTimeType { get; set; }

    /// <summary>
    /// Left bound of the time period over which average measurements were calculated, inclusive, always UTC
    /// </summary>
    public DateTimeOffset? FromDateTime { get; set; }

    /// <summary>
    /// List of indexes calculated from the values available. Indexes are defined by relevant national and international institutions, e.g. EU, GIOŚ or US EPA
    /// </summary>
    
    //!! Allways AIRLY_CAQI
    //public List<Index> Indexes { get; set; }

    /// <summary>
    /// List of 'standard' values, or 'limits' for pollutants that should not be exceeded over certain period of time. Limits are defined by relevant national and international institutions, like e.g. WHO or EPA. For each standard limit in this list there is also a corresponding measurement expressed as a percent value of the limit
    /// </summary>
    public List<StandardEntity> Standards { get; set; }

    /// <summary>
    /// Right bound of the time period over which average measurements were calculated, exclusive, always UTC
    /// </summary>
    public DateTimeOffset? TillDateTime { get; set; }

    /// <summary>
    /// List of raw measurements, averaged over specified period. Measurement types available in this list depend on the capabilities of the queried installation, e.g. particulate matter (PM1, PM25, PM10), gases (CO, NO2, SO2, O3) or weather conditions (temperature, humidity, pressure)
    /// </summary>
    public List<ValuePairEntity> Values { get; set; }

}

[Table("Standards")]
public class StandardEntity
{
    [Key]
    public int StandardId { get; set; }

    public int AveragedValueEntityFk { get; set; }

    [ForeignKey(nameof(AveragedValueEntityFk))]
    public AveragedValueEntity AveragedValue { get; set; }

    /// <summary>
    /// Averaging period this standard is applied for
    /// </summary>
    public string Averaging { get; set; }

    /// <summary>
    /// Limit value of the pollutant
    /// </summary>
    public double? Limit { get; set; }

    /// <summary>
    /// Name of this standard
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Pollutant measurement as percent of allowable limit
    /// </summary>
    public double? Percent { get; set; }

    /// <summary>
    /// Pollutant described by this standard
    /// </summary>
    public string Pollutant { get; set; }

}

[Table("ValuePairs")]

public class ValuePairEntity
{
    [Key]
    public int ValuePairId { get; set; }

    public int AveragedValueEntityFk { get; set; }

    [ForeignKey(nameof(AveragedValueEntityFk))]
    public AveragedValueEntity AveragedValue { get; set; }


    /// <summary>
    /// Name of this measurement
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Value of this measurement
    /// </summary>
    public double? Value { get; set; }
}