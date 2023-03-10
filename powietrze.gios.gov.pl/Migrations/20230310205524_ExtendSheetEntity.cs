using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace powietrze.gios.gov.pl.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSheetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullSheetName",
                table: "SheetEntities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeRange",
                table: "SheetEntities",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullSheetName",
                table: "SheetEntities");

            migrationBuilder.DropColumn(
                name: "TimeRange",
                table: "SheetEntities");
        }
    }
}
