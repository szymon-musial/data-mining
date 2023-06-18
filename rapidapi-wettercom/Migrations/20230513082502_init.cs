using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rapidapi_wettercom.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rapidapi");

            migrationBuilder.CreateTable(
                name: "RapidForecast",
                schema: "rapidapi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Weather_State = table.Column<int>(type: "integer", nullable: false),
                    Weather_Text = table.Column<string>(type: "text", nullable: false),
                    Weather_Icon = table.Column<string>(type: "text", nullable: false),
                    Prec_Sum = table.Column<double>(type: "double precision", nullable: false),
                    Prec_Probability = table.Column<double>(type: "double precision", nullable: false),
                    Prec_Class = table.Column<int>(type: "integer", nullable: false),
                    Temperature_Avg = table.Column<int>(type: "integer", nullable: false),
                    Clouds_High = table.Column<int>(type: "integer", nullable: false),
                    Clouds_Low = table.Column<int>(type: "integer", nullable: false),
                    Clouds_Middle = table.Column<int>(type: "integer", nullable: false),
                    Wind_Unit = table.Column<string>(type: "text", nullable: false),
                    Wind_Direction = table.Column<string>(type: "text", nullable: false),
                    Wind_Text = table.Column<string>(type: "text", nullable: false),
                    Wind_Avg = table.Column<int>(type: "integer", nullable: false),
                    Wind_SignificationWind = table.Column<bool>(type: "boolean", nullable: false),
                    Windchill_Avg = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Period = table.Column<int>(type: "integer", nullable: false),
                    FreshSnow = table.Column<int>(type: "integer", nullable: true),
                    SunHours = table.Column<int>(type: "integer", nullable: true),
                    RainHours = table.Column<int>(type: "integer", nullable: true),
                    Pressure = table.Column<int>(type: "integer", nullable: false),
                    RelativeHumidity = table.Column<int>(type: "integer", nullable: false),
                    IsNight = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapidForecast", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RapidForecast",
                schema: "rapidapi");
        }
    }
}
