using danepubliczne.imgw.pl.Entities;
using Microsoft.EntityFrameworkCore;

namespace danepubliczne.imgw.pl.Persistence;


public class AppDbContext : DbContext
{
    public static string connectionString = "Host=localhost;Database=DataMining;Username=postgres;Password=root";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<WatherDataEntity> WatherData { get; set; }
}
