namespace airly_data_fetch;

public class Measurements
{
    public AveragedValues Current { get; set; }

    public ICollection<AveragedValues> Forecast { get; set; }

    public ICollection<AveragedValues> History { get; set; }

}

public class AveragedValues
{
    /// <summary>
    /// Left bound of the time period over which average measurements were calculated, inclusive, always UTC
    /// </summary>
    public DateTimeOffset? FromDateTime { get; set; }

    /// <summary>
    /// List of indexes calculated from the values available. Indexes are defined by relevant national and international institutions, e.g. EU, GIOŚ or US EPA
    /// </summary>
    public ICollection<Index> Indexes { get; set; }

    /// <summary>
    /// List of 'standard' values, or 'limits' for pollutants that should not be exceeded over certain period of time. Limits are defined by relevant national and international institutions, like e.g. WHO or EPA. For each standard limit in this list there is also a corresponding measurement expressed as a percent value of the limit
    /// </summary>
    public ICollection<Standard> Standards { get; set; }

    /// <summary>
    /// Right bound of the time period over which average measurements were calculated, exclusive, always UTC
    /// </summary>
    public DateTimeOffset? TillDateTime { get; set; }

    /// <summary>
    /// List of raw measurements, averaged over specified period. Measurement types available in this list depend on the capabilities of the queried installation, e.g. particulate matter (PM1, PM25, PM10), gases (CO, NO2, SO2, O3) or weather conditions (temperature, humidity, pressure)
    /// </summary>
    public ICollection<ValuePair> Values { get; set; }

}

public class Standard
{
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

public class ValuePair
{
    /// <summary>
    /// Name of this measurement
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Value of this measurement
    /// </summary>
    public double? Value { get; set; }
}