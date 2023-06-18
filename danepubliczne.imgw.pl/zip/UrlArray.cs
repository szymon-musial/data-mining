namespace danepubliczne.imgw.pl.zip;

public class UrlArray
{
    const int StartYear = 2005;
    const int StopYear = 2023;

    readonly IEnumerable<int> yearsRange = Enumerable.Range(StartYear, count: StopYear - StartYear);
    readonly IEnumerable<int> monthsRange = Enumerable.Range(1, count: 12);


    public string urlScheme = "https://danepubliczne.imgw.pl/data/dane_pomiarowo_obserwacyjne/dane_meteorologiczne/terminowe/klimat/{0}/{0}_{1}_k.zip";

    // 14.03.2023
    public IEnumerable<int> monthInCurrentYear = new[] { 1 };

    public IEnumerable<string> Urls
    {
        get
        {
            var result = yearsRange.SelectMany(year => monthsRange.Select(month => string.Format(urlScheme, year, month.ToString("D2"))));

            result = result.Concat(monthInCurrentYear.Select(month => string.Format(urlScheme, 2023, month.ToString("D2"))));

            return result;
        }
    }
}
