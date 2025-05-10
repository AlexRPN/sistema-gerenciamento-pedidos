using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Funcionario.Request
{
    public class FuncionarioEdicaoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public PerfilAcessoEnum? PerfilAcesso { get; set; }
        public EnderecoFuncionarioEdicaoDto? Endereco { get; set; }
    }
}
