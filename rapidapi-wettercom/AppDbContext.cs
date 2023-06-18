using Microsoft.EntityFrameworkCore;
using rapidapi_wettercom;

namespace rapidapi_wettercom;

public class AppDbContext : DbContext
{
    public static string connectionString = "Host=localhost;Database=DataMining;Username=postgres;Password=root";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("rapidapi");
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<RapidForecastDbModel> RapidForecast { get; set; }
}
