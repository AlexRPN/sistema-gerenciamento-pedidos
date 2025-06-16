using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Request;
using sistema_gerenciamento_pedidos.Models.TelefoneClientes;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Request
{
    public class ClienteEdicaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        
        public EnderecoClienteEdicaoDto? EnderecoCliente { get; set; }
        public TelefoneClienteEdicaoDto Telefone { get; set; }
    }
}
