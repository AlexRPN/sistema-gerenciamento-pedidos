using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.PedidoProduto;

namespace sistema_gerenciamento_pedidos.Models.Pedidos
{
    public class PedidoModel
    {
        public int Id { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public TipoPagamentoEnum TipoPagamento { get; set; }
        public StatusPedidoEnum StatusPedido { get; set; } = StatusPedidoEnum.EmPreparacao;
        public string? MotivoCancelamento { get; set; }

        //Relacionamento entre as tabelas CLIENTE x PEDIDO
        public int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; }

        public ICollection<PedidoProdutoModel> PedidoProdutos { get; set; }
    }
}
