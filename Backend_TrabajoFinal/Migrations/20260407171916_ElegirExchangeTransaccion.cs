using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_TrabajoFinal.Migrations
{
    /// <inheritdoc />
    public partial class ElegirExchangeTransaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Exchange",
                table: "Transacciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "Transacciones");
        }
    }
}
