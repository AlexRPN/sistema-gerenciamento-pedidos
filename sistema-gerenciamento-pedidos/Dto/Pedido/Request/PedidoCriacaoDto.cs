using sistema_gerenciamento_pedidos.Dto.PedidoProduto.Request;

namespace sistema_gerenciamento_pedidos.Dto.Pedido.Request
{
    public class PedidoCriacaoDto
    {
        public int ClienteId { get; set; }
        public List<PedidoProdutoCriacaoDto> PedidoProdutos { get; set; }
    }
}
