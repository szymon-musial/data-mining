using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace powietrze.gios.gov.pl.Migrations
{
    /// <inheritdoc />
    public partial class ExtendStationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Used",
                table: "StationInFileEntities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Used",
                table: "StationInFileEntities");
        }
    }
}
