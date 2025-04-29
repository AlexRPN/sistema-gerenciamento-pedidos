namespace sistema_gerenciamento_pedidos.Dto.PedidoProduto.Response
{
    public class PedidoProdutoResponse
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public string Observacao { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
