using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace danepubliczne.imgw.pl.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatherData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StationCode = table.Column<int>(type: "integer", nullable: false),
                    StationName = table.Column<string>(type: "text", nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    TemperatureMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    WetBulbTemperature = table.Column<double>(type: "double precision", nullable: true),
                    WetBulbTemperatureMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    IceIndicator = table.Column<string>(type: "text", nullable: true),
                    VentilationIndicator = table.Column<string>(type: "text", nullable: true),
                    RelativeHumidity = table.Column<int>(type: "integer", nullable: true),
                    RelativeHumidityMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    WindDirectionCode = table.Column<string>(type: "text", nullable: true),
                    WindDirectionMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    WindSpeed = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeedMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    CloudCover = table.Column<int>(type: "integer", nullable: true),
                    CloudCoverMeasurementStatus = table.Column<string>(type: "text", nullable: true),
                    VisibilityCode = table.Column<string>(type: "text", nullable: true),
                    VisibilityMeasurementStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatherData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatherData");
        }
    }
}
