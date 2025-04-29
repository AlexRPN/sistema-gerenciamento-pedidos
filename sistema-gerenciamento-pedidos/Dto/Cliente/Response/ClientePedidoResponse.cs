using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Response
{
    public class ClientePedidoResponse
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public EnderecoClienteResponse Endereco { get; set; }
    }
}
