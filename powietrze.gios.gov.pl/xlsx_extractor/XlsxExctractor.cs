using IronXL;
using powietrze.gios.gov.pl.Entities;
using powietrze.gios.gov.pl.Persistence;
using powietrze.gios.gov.pl.zip;
using System.Data;

namespace powietrze.gios.gov.pl.xlsxExtractor;

public class XlsxExctractor
{
    private readonly string _destinationFolder;
    private readonly AppDbContext _appDbContext;

    int MaxColumnToSearch = 10;
    int ColumnDescriptionIndex = 0;

    public XlsxExctractor(string destinationFolder, AppDbContext appDbContext)
    {
        _destinationFolder = destinationFolder;
        _appDbContext = appDbContext;
    }

    public int? SearchRowsInColumnForValue(DataTable table, string value)
    {
        for (int rowIndex = 0; rowIndex < MaxColumnToSearch; rowIndex++)
        {
            var cellInFirstColumn = table.Rows[rowIndex][ColumnDescriptionIndex];
            if (cellInFirstColumn is string cellInFirstColumnString)
            {
                if (cellInFirstColumnString == value)
                {
                    return rowIndex;
                }
            }

        }
        return null;
    }

    public int? GetStationCodeRowIndex(DataTable table) => SearchRowsInColumnForValue(table, "Kod stacji");
    public int? GetPollutantNameRowIndex(DataTable table) => SearchRowsInColumnForValue(table, "Wskaźnik");

    public IDictionary<int, string> GetStationNames(DataTable table, int stationCodeRowIndex)
    {
        var stationDictionary = new Dictionary<int, string>();

        for (int columnIndex = ColumnDescriptionIndex + 1; columnIndex < table.Columns.Count; columnIndex++)
        {
            if (table.Rows[stationCodeRowIndex][columnIndex] is string stationNameCell)
            {
                stationDictionary.Add(columnIndex, stationNameCell);
            }
        }

        return stationDictionary;
    }

    private int? GetMeasurementFirstRowIndex(DataTable table)
    {
        for (int rowIndex = 0; rowIndex < MaxColumnToSearch; rowIndex++)
        {
            var cellInFirstColumn = table.Rows[rowIndex][ColumnDescriptionIndex];
            if (cellInFirstColumn is DateTime)
            {
                return rowIndex;
            }

        }
        return null;
    }

    private IEnumerable<SheetEntity> InsertDataForStation(
        DataTable table,
        KeyValuePair<int, string> keyValuePair,
        int stationCodeRowIndex,
        int pollutantCodeRowIndex,
        int measurementStartingRowIndex)
    {
        var stationColumn = keyValuePair.Key;
        var stationName = keyValuePair.Value;

        var rowCount = table.Columns[keyValuePair.Key].Table.Rows.Count;

        var pollutionName = (string)table.Rows[pollutantCodeRowIndex][stationColumn];


        for (int currentRow = measurementStartingRowIndex; currentRow < rowCount; currentRow++)
        {
            // DataTime UTC must have when works with Pg
            var dateTime = DateTime.SpecifyKind((DateTime)table.Rows[currentRow][ColumnDescriptionIndex], DateTimeKind.Utc);

            double? castedValue = null;

            var rawCell = table.Rows[currentRow][stationColumn];
            if (rawCell is DBNull)
            {
                castedValue = null;
            }

            if (rawCell is double doubleCell)
            {
                castedValue = doubleCell;
            }

            yield return new SheetEntity()
            {
                PollutantName = pollutionName,
                StationName = stationName,
                Value = castedValue,
                Date = dateTime,
            };
        }
    }

    public IDictionary<int, string> FilterStationNames(IDictionary<int, string> stationNames)
        =>  stationNames.Where(s =>
                UsedStations.ExactStationName.Contains(s.Value) ||
                UsedStations.ContainsStationName.Any(pattern => s.Value.Contains(pattern))
            ).ToDictionary(i => i.Key, i => i.Value);

    public void DbSeed(int maxRunningTasks = int.MaxValue)
    {
        string extractPath = Path.Combine(_destinationFolder, "extracted_files");
        var allWorkbooks = Directory.GetDirectories(extractPath).SelectMany(Directory.GetFiles);

        var tasks = CreateProcessingTasks(allWorkbooks);

        Parallel.ForEach(tasks, new ParallelOptions { MaxDegreeOfParallelism = maxRunningTasks }, a => a.Wait());
    }

    public IEnumerable<Task> CreateProcessingTasks(IEnumerable<string> allWorkbooks)
    {
        foreach (var workbookPath in allWorkbooks)
        {
            yield return ProcessFile(workbookPath, allWorkbooks.Count());
        }
    }

    static int OpenedFiles = 0;
    static int ProcessedFiles = 0;
    static int ErrorFilesCount = 0;


    public Task ProcessFile(string WrokBookPath, int filesCount)
    {
        return Task.Run(() =>
        {
            var fileName = Path.GetFileName(WrokBookPath);

            try
            {
                Console.WriteLine($"Loading {fileName}.");

                OpenedFiles++;

                WorkBook workBook = WorkBook.Load(WrokBookPath);
                WorkSheet workSheet = workBook.WorkSheets.First();

                DataSet dataSet = workBook.ToDataSet();
                var table = dataSet.Tables[0];

                Console.WriteLine($"Processing {OpenedFiles}/{filesCount} file: {fileName}");


                var stationCodeRowIndex = GetStationCodeRowIndex(table)!;
                var pollutantCodeRowIndex = GetPollutantNameRowIndex(table)!;
                var measurementStartingRowIndex = GetMeasurementFirstRowIndex(table)!;

                var allStations = GetStationNames(table, stationCodeRowIndex.Value);

                var useStationNames = FilterStationNames(allStations);

                var enitiesToAdd = new List<SheetEntity>();

                foreach (var station in useStationNames)
                {
                    enitiesToAdd.AddRange(InsertDataForStation(table, station, stationCodeRowIndex.Value, pollutantCodeRowIndex.Value, measurementStartingRowIndex.Value));
                }

                lock (_appDbContext)
                {
                    ProcessedFiles++;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Uploading {ProcessedFiles}/{filesCount}");
                    Console.ResetColor();

                    _appDbContext.AddRange(enitiesToAdd);
                    int updateCount = _appDbContext.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Finished {ProcessedFiles}/{filesCount}. Added {updateCount} rows");
                    Console.ResetColor();
                }

                table.Dispose();
                dataSet.Dispose();
                workBook.Close();

            }
            catch (Exception)
            {
                ErrorFilesCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error ocured whan process {fileName}");
                Console.WriteLine($"Failed files count {ErrorFilesCount}/{filesCount}");
                Console.ResetColor();
            }
        });
    }
}


public static class ListExtensions
{
    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}