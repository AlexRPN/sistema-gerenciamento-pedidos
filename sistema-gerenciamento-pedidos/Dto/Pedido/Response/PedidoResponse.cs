using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.PedidoProduto.Response;
using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Pedido.Response
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public StatusPedidoEnum StatusPedido { get; set; }
        public ClientePedidoResponse Cliente { get; set; }
        public List<PedidoProdutoResponse> Produtos { get; set; } = new();
    }
}
