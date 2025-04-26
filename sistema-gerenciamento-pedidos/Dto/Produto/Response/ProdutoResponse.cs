using sistema_gerenciamento_pedidos.Dto.Empresa.Response;

namespace sistema_gerenciamento_pedidos.Dto.Produto.Response
{
    public class ProdutoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Categoria { get; set; }
        public string Tamanho { get; set; }
        public string Situacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? Imagem { get; set; } = string.Empty;
        public EmpresaResponse Empresa { get; set; }
    }
}
