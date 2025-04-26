using sistema_gerenciamento_pedidos.Models.Pedidos;
using sistema_gerenciamento_pedidos.Models.Produtos;
using System.Text.Json.Serialization;

namespace sistema_gerenciamento_pedidos.Models.PedidoProduto
{
    public class PedidoProdutoModel
    {
        [JsonIgnore]
        public int PedidoId { get; set; }
        public PedidoModel Pedido { get; set; }

        [JsonIgnore]
        public int ProdutoId { get; set; }
        public ProdutoModel Produto { get; set; }

        public int Quantidade { get; set; }
        public string Observacao { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
