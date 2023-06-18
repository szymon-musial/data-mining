using Microsoft.EntityFrameworkCore;

namespace airly_data_fetch;

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
        modelBuilder.HasDefaultSchema("airly");
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<MeasurementEntity> MeasurementEntities { get; set; }
}
