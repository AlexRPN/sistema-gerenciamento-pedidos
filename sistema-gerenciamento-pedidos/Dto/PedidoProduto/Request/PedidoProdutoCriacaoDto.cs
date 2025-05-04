namespace sistema_gerenciamento_pedidos.Dto.PedidoProduto.Request
{
    public class PedidoProdutoCriacaoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public decimal ValorUnitario { get; set; }
    }
}
