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

    /// <summary>
    /// 2016 and few in 2018 year are diferrent
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public int? GetStationCodeRowIndex2ndTry(DataTable table)
    {
        for (int rowIndex = 0; rowIndex < MaxColumnToSearch; rowIndex++)
        {
            var cellInFirstColumn = table.Rows[rowIndex][ColumnDescriptionIndex+1];
            if (cellInFirstColumn is string cellInFirstColumnString)
            {
                // guess only its station name
                if (cellInFirstColumnString.Length > 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"Assume it's station name {cellInFirstColumnString}");                    
                    Console.ResetColor();
                    return rowIndex;
                }
            }

        }
        return null;
    }


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
        string workbookFullName,
        int measurementStartingRowIndex)
    {
        var stationColumn = keyValuePair.Key;
        var stationName = keyValuePair.Value;

        var rowCount = table.Columns[keyValuePair.Key].Table.Rows.Count;

        var shortFileName = Path.GetFileNameWithoutExtension(workbookFullName).Trim();
        var splitedFileName = shortFileName.Split('_');

        var pollutionName = splitedFileName[1];
        var timeRange = int.Parse(splitedFileName[2].Remove(splitedFileName[2].Length - 1));


        for (int currentRow = measurementStartingRowIndex; currentRow < rowCount; currentRow++)
        {
            // case 2014_C6H6_1g.xlsx
            // user create table so (index < rowCount) can be null


            // DataTime UTC must have when works with Pg
            var rawDataTime = table.Rows[currentRow][ColumnDescriptionIndex];
            if(rawDataTime is DBNull)
            {
                continue;
            }
            var dateTime = DateTime.SpecifyKind((DateTime)rawDataTime, DateTimeKind.Utc);

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
                FullSheetName = workbookFullName,
                TimeRange = timeRange,
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

        var filterredWorkbooks = allWorkbooks.Where(b => !b.Contains("epozycja"));
        var tasks = CreateProcessingTasks(filterredWorkbooks);

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

                var stationCodeRowIndex = GetStationCodeRowIndex(table);
                if(stationCodeRowIndex is null )
                {
                    stationCodeRowIndex = GetStationCodeRowIndex2ndTry(table);
                }

                var measurementStartingRowIndex = GetMeasurementFirstRowIndex(table)!;

                var allStations = GetStationNames(table, stationCodeRowIndex.Value);

                var useStationNames = FilterStationNames(allStations);

                SaveStations(fileName, allStations, useStationNames);


                var enitiesToAdd = new List<SheetEntity>();

                foreach (var station in useStationNames)
                {
                    enitiesToAdd.AddRange(InsertDataForStation(table, station, stationCodeRowIndex.Value, fileName, measurementStartingRowIndex.Value));
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

    public void SaveStations(string workBook, IDictionary<int, string> allStations, IDictionary<int, string> usedStations)
    {
        string workBookWithoutYear = Path.GetFileNameWithoutExtension(workBook).Remove(0, 5);

        var usedStationsList = usedStations.Select(i => i.Value);
        var allStationsList = allStations.Select(i => i.Value);
        var notUsedStationList = allStationsList.Except(usedStationsList);

        lock (_appDbContext)
        {
            _appDbContext.StationInFileEntities.AddRange(usedStationsList.Select(i => new StationInFileEntity
            {
                FullSheetName = workBook,
                SheetName = workBookWithoutYear,
                StationName = i,
                Used = true
            }));

            _appDbContext.StationInFileEntities.AddRange(notUsedStationList.Select(i => new StationInFileEntity
            {
                FullSheetName = workBook,
                SheetName = workBookWithoutYear,
                StationName = i,
                Used = false
            }));
        }
    }

}
