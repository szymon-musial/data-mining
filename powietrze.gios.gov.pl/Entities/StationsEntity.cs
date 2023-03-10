using System.ComponentModel.DataAnnotations;

namespace powietrze.gios.gov.pl.Entities;

public class StationInFileEntity
{
    [Key]
    public int Id { get; set; }
    public string StationName { get; set; }
    public string SheetName { get; set; }
    public string FullSheetName { get; set; }
    public bool Used { get; set; }
}
