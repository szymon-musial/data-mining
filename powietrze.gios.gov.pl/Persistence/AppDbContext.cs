using Microsoft.EntityFrameworkCore;
using powietrze.gios.gov.pl.Entities;
using System;

namespace powietrze.gios.gov.pl.Persistence;


public class AppDbContext : DbContext
{
    public static string connectionString = "Host=localhost;Database=DataMining;Username=postgres;Password=root";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<SheetEntity> SheetEntities { get; set; }
}
