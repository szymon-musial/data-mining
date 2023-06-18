using Mapster;

namespace airly_data_fetch;

public class AppRepostioty
{
    readonly AppDbContext _appDbContext;
    public AppRepostioty(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void InsertEntity(Measurements airlyMeasurement, KeyValuePair<int, string> friendlyStationName)
    {
        var measurmentEntity = airlyMeasurement.Adapt<MeasurementEntity>();
        measurmentEntity.RequestDate = DateTime.UtcNow.Date;
        measurmentEntity.RequestTime = DateTime.UtcNow.TimeOfDay;
        measurmentEntity.StationId = friendlyStationName.Key;
        measurmentEntity.StationName = friendlyStationName.Value;

        _appDbContext.MeasurementEntities.Add(measurmentEntity);
        var entitiesCount = _appDbContext.SaveChanges();
        Console.WriteLine($"Added {entitiesCount} entities");
    }

}
