using Microsoft.EntityFrameworkCore;
using powietrze.gios.gov.pl.Entities;
using System;

namespace powietrze.gios.gov.pl.Persistence;


public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var defaultPgConnectionString = "Host=localhost;Database=DataMining;Username=postgres;Password=root";
        optionsBuilder.UseNpgsql(defaultPgConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<SheetEntity> SheetEntities { get; set; }
}
