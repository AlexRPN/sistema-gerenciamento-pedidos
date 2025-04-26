using sistema_gerenciamento_pedidos.Dto.Empresa.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Response;

namespace sistema_gerenciamento_pedidos.Dto.Funcionario.Response
{
    public class FuncionarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string NomeUsuario { get; set; }
        public string Situacao { get; set; }
        public string DataCriacao { get; set; }
        public string PerfilAcesso { get; set; }
        public EmpresaResponse Empresa { get; set; }
        public EnderecoFuncionarioResponse Endereco { get; set; }
    }
}
