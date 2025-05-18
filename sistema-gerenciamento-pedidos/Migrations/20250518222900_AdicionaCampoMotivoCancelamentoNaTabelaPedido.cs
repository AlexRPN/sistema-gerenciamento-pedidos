using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_gerenciamento_pedidos.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoMotivoCancelamentoNaTabelaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelamento",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoCancelamento",
                table: "Pedido");
        }
    }
}
