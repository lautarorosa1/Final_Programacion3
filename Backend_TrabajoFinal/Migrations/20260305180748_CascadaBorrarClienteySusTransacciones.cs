using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_TrabajoFinal.Migrations
{
    /// <inheritdoc />
    public partial class CascadaBorrarClienteySusTransacciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_ClienteId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_ClienteId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Transacciones");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ClientId",
                table: "Transacciones",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_ClientId",
                table: "Transacciones",
                column: "ClientId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_ClientId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_ClientId",
                table: "Transacciones");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ClienteId",
                table: "Transacciones",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_ClienteId",
                table: "Transacciones",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }
    }
}
