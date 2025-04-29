using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_gerenciamento_pedidos.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNomeColunaValorTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnderecoFuncionario_FuncionarioId",
                table: "EnderecoFuncionario");

            migrationBuilder.DropIndex(
                name: "IX_EnderecoCliente_ClienteId",
                table: "EnderecoCliente");

            migrationBuilder.RenameColumn(
                name: "ValorUnitario",
                table: "Pedido",
                newName: "ValorTotal");

            migrationBuilder.AlterColumn<string>(
                name: "Imagem",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoFuncionario_FuncionarioId",
                table: "EnderecoFuncionario",
                column: "FuncionarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoCliente_ClienteId",
                table: "EnderecoCliente",
                column: "ClienteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnderecoFuncionario_FuncionarioId",
                table: "EnderecoFuncionario");

            migrationBuilder.DropIndex(
                name: "IX_EnderecoCliente_ClienteId",
                table: "EnderecoCliente");

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "Pedido",
                newName: "ValorUnitario");

            migrationBuilder.AlterColumn<string>(
                name: "Imagem",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoFuncionario_FuncionarioId",
                table: "EnderecoFuncionario",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoCliente_ClienteId",
                table: "EnderecoCliente",
                column: "ClienteId");
        }
    }
}
