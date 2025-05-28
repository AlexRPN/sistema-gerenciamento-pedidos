using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_gerenciamento_pedidos.Migrations
{
    /// <inheritdoc />
    public partial class CriaCampoTipoEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoEntrega",
                table: "Pedido",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoEntrega",
                table: "Pedido");
        }
    }
}
