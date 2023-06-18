using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace airly_data_fetch.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "airly");

            migrationBuilder.CreateTable(
                name: "Measurements",
                schema: "airly",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StationId = table.Column<int>(type: "integer", nullable: false),
                    StationName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                });

            migrationBuilder.CreateTable(
                name: "AveragedValues",
                schema: "airly",
                columns: table => new
                {
                    AveragedValuesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeasurementEntityFk = table.Column<int>(type: "integer", nullable: false),
                    AveragedValueTimeType = table.Column<int>(type: "integer", nullable: false),
                    FromDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TillDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AveragedValues", x => x.AveragedValuesId);
                    table.ForeignKey(
                        name: "FK_AveragedValues_Measurements_MeasurementEntityFk",
                        column: x => x.MeasurementEntityFk,
                        principalSchema: "airly",
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                schema: "airly",
                columns: table => new
                {
                    StandardId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AveragedValueEntityFk = table.Column<int>(type: "integer", nullable: false),
                    Averaging = table.Column<string>(type: "text", nullable: true),
                    Limit = table.Column<double>(type: "double precision", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Percent = table.Column<double>(type: "double precision", nullable: true),
                    Pollutant = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standards", x => x.StandardId);
                    table.ForeignKey(
                        name: "FK_Standards_AveragedValues_AveragedValueEntityFk",
                        column: x => x.AveragedValueEntityFk,
                        principalSchema: "airly",
                        principalTable: "AveragedValues",
                        principalColumn: "AveragedValuesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValuePairs",
                schema: "airly",
                columns: table => new
                {
                    ValuePairId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AveragedValueEntityFk = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValuePairs", x => x.ValuePairId);
                    table.ForeignKey(
                        name: "FK_ValuePairs_AveragedValues_AveragedValueEntityFk",
                        column: x => x.AveragedValueEntityFk,
                        principalSchema: "airly",
                        principalTable: "AveragedValues",
                        principalColumn: "AveragedValuesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AveragedValues_MeasurementEntityFk",
                schema: "airly",
                table: "AveragedValues",
                column: "MeasurementEntityFk");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_AveragedValueEntityFk",
                schema: "airly",
                table: "Standards",
                column: "AveragedValueEntityFk");

            migrationBuilder.CreateIndex(
                name: "IX_ValuePairs_AveragedValueEntityFk",
                schema: "airly",
                table: "ValuePairs",
                column: "AveragedValueEntityFk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Standards",
                schema: "airly");

            migrationBuilder.DropTable(
                name: "ValuePairs",
                schema: "airly");

            migrationBuilder.DropTable(
                name: "AveragedValues",
                schema: "airly");

            migrationBuilder.DropTable(
                name: "Measurements",
                schema: "airly");
        }
    }
}
