using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.Funcionarios;

namespace sistema_gerenciamento_pedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmpresaModel> Empresa { get; set; }
        public DbSet<FuncionarioModel> Funcionario { get; set; }
    }
}
