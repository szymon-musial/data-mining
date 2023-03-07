using Microsoft.EntityFrameworkCore;
using powietrze.gios.gov.pl.Entities;
using System;

namespace powietrze.gios.gov.pl.Persistence;


public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var DbHost = "localhost";
        var DbUserName = "postgres";
        var DbPassword = "root";

        var defaultPgConnectionString = $"Host={DbHost};Database=DataMining;Username={DbUserName};Password={DbPassword}";
        optionsBuilder.UseNpgsql(defaultPgConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<SheetEntity> SheetEntities { get; set; }
}
