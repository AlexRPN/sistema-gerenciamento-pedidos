using sistema_gerenciamento_pedidos.Dto.PedidoProduto.Request;
using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Pedido.Request
{
    public class PedidoEdicaoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public List<PedidoProdutoCriacaoDto> PedidoProdutos { get; set; }
    }
}
