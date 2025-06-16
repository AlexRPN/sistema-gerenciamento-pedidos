using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Models.Funcionarios;
using sistema_gerenciamento_pedidos.Models.PedidoProduto;
using sistema_gerenciamento_pedidos.Models.Pedidos;
using sistema_gerenciamento_pedidos.Models.Produtos;
using sistema_gerenciamento_pedidos.Models.TelefoneClientes;

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
        public DbSet<PedidoProdutoModel> PedidoProduto { get; set; }
        public DbSet<TelefoneClienteDto> Telefone { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da tabela PedidoProduto
            modelBuilder.Entity<PedidoProdutoModel>(entity =>
            {
                // Configurar chave composta
                entity.HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

                // Relacionamento com Pedido
                entity.HasOne(pp => pp.Pedido)
                      .WithMany(p => p.PedidoProdutos) // Propriedade de navegação no PedidoModel
                      .HasForeignKey(pp => pp.PedidoId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita cascata que gera conflito

                // Relacionamento com Produto
                entity.HasOne(pp => pp.Produto)
                      .WithMany(prod => prod.PedidoProdutos) // Propriedade de navegação no ProdutoModel
                      .HasForeignKey(pp => pp.ProdutoId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita cascata que gera conflito

                // Nome da tabela no banco (opcional)
                entity.ToTable("PedidoProduto");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
