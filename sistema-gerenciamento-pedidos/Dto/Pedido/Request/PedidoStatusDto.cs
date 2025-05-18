using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Pedido.Request
{
    public class PedidoStatusDto
    {
        public int Id { get; set; }
        public StatusPedidoEnum StatusPedido { get; set; }
        public string? MotivoCancelamento { get; set; }
    }
}
