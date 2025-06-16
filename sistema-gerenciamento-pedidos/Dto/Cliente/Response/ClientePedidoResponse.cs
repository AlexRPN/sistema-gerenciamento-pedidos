using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Response;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Response
{
    public class ClientePedidoResponse
    {
        public string Nome { get; set; }
        public TelefoneClienteResponse Telefone { get; set; }
        public EnderecoClienteResponse Endereco { get; set; }
    }
}
