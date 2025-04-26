using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_gerenciamento_pedidos.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirNomeValorUnitario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "Pedido",
                newName: "ValorUnitario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorUnitario",
                table: "Pedido",
                newName: "ValorTotal");
        }
    }
}
