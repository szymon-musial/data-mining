using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using powietrze.gios.gov.pl.Entities;
using danepubliczne.imgw.pl.Persistence;
using danepubliczne.imgw.pl.Entities;
using System.Text;

namespace danepubliczne.imgw.pl.csv_extractor;


public class CsvExtractor
{
    private readonly string _destinationFolder;
    private readonly AppDbContext _appDbContext;


    public CsvExtractor(string destinationFolder, AppDbContext appDbContext)
    {
        _destinationFolder = destinationFolder;
        _appDbContext = appDbContext;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public void DbSeed(int maxRunningTasks = int.MaxValue)
    {
        string extractPath = Path.Combine(_destinationFolder, "extracted_files");
        var allCsv = Directory.GetFiles(extractPath);

        var tasks = CreateProcessingTasks(allCsv);

        Parallel.ForEach(tasks, new ParallelOptions { MaxDegreeOfParallelism = maxRunningTasks }, a => a.Wait());
    }

    public IEnumerable<Task> CreateProcessingTasks(IEnumerable<string> allCsv)
    {
        foreach (var currentCsv in allCsv)
        {
            yield return ProcessFile(currentCsv, allCsv.Count());
        }
    }

    static int OpenedFiles = 0;
    static int ProcessedFiles = 0;
    static int ErrorFilesCount = 0;
    static int UploadingCount = 0;

    public Task ProcessFile(string csvPath, int filesCount)
    {
        return Task.Run(() =>
        {
            var fileName = Path.GetFileName(csvPath);
            try
            {
                Console.WriteLine($"Loading {fileName}.");
                OpenedFiles++;
                Console.WriteLine($"Processing {OpenedFiles}/{filesCount} file: {fileName}");


                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (var reader = new StreamReader(csvPath, Encoding.GetEncoding("windows-1250")))
                using (var csv = new CsvReader(reader, configuration))
                {
                    var records = csv.GetRecords<CSVEntity>();
                    InsertRecordsFromCsv(records, filesCount);
                }
            }
            catch (Exception e)
            {
                ErrorFilesCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error ocured whan process {fileName}");
                Console.WriteLine(e);
                Console.WriteLine($"Failed files count {ErrorFilesCount}/{filesCount}");
                Console.ResetColor();
            }
        });
    }



    public void InsertRecordsFromCsv(IEnumerable<CSVEntity> cSVEntities, int filesCount)
    {
        IEnumerable<WatherDataEntity> records =

            cSVEntities.Select(c => new WatherDataEntity()
            {
                StationCode = c.StationCode,
                StationName = c.StationName,
                Date = new DateTime(c.Year, c.Month, c.Day, c.Hour, 0, 0, DateTimeKind.Utc), // Date Time UTC is important for PG starting from .net 6
                Temperature = c.Temperature,
                TemperatureMeasurementStatus = c.TemperatureMeasurementStatus,
                WetBulbTemperature = c.WetBulbTemperature,
                WetBulbTemperatureMeasurementStatus = c.WetBulbTemperatureMeasurementStatus,
                IceIndicator = c.IceIndicator,
                VentilationIndicator = c.VentilationIndicator,
                RelativeHumidity = c.RelativeHumidity,
                RelativeHumidityMeasurementStatus = c.RelativeHumidityMeasurementStatus,
                WindDirectionCode = c.WindDirectionCode,
                WindDirectionMeasurementStatus = c.WindDirectionMeasurementStatus,
                WindSpeed = c.WindSpeed,
                WindSpeedMeasurementStatus = c.WindSpeedMeasurementStatus,
                CloudCover = c.CloudCover,
                CloudCoverMeasurementStatus = c.CloudCoverMeasurementStatus,
                VisibilityCode = c.VisibilityCode,
                VisibilityMeasurementStatus = c.VisibilityMeasurementStatus,
            });

        ProcessedFiles++;

        lock (_appDbContext)
        {
            UploadingCount++;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Uploading {UploadingCount}/{filesCount}");
            Console.ResetColor();

            _appDbContext.WatherData.AddRange(records);
            int updateCount = _appDbContext.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Finished {ProcessedFiles}/{filesCount}. Added {updateCount} rows");
            Console.ResetColor();
        }
    }

}

