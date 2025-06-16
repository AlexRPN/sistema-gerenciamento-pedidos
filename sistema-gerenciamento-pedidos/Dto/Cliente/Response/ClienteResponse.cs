using sistema_gerenciamento_pedidos.Dto.Empresa.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Response;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Response
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TelefoneClienteResponse Telefone { get; set; }
        public EnderecoClienteResponse Endereco { get; set; }
        public string Situacao { get; set; }
        public string PerfilAcesso { get; set; }
        public DateTime DataCadastro { get; set; }

        public EmpresaResponse? Empresa { get; set; }
    }
}
