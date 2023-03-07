using System.ComponentModel.DataAnnotations;

namespace powietrze.gios.gov.pl.Entities;

public class SheetEntity
{
    [Key]
    public int Id { get; set; }
    public string PollutantName { get; set; }

    public string StationName { get; set; }
    public double? Value { get; set; }

    public DateTime Date { get; set; }

}
