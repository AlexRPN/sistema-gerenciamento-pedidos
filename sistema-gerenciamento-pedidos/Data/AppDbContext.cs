using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Models.Funcionarios;
using sistema_gerenciamento_pedidos.Models.Pedidos;
using sistema_gerenciamento_pedidos.Models.Produtos;

namespace sistema_gerenciamento_pedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmpresaModel> Empresa { get; set; }
        public DbSet<FuncionarioModel> Funcionario { get; set; }
        public DbSet<EnderecoFuncionarioModel> EnderecoFuncionario { get; set; }
        public DbSet<ClienteModel> Cliente { get; set; }
        public DbSet<EnderecoClienteModel> EnderecoCliente { get; set; }
        public DbSet<PedidoModel> Pedido { get; set; }
        public DbSet<ProdutoModel> Produto { get; set; }
    }
}
