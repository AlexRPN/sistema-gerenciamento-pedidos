using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Produto.Request
{
    public class ProdutoEdicaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public TamanhoEnum? Tamanho { get; set; }
        public SituacaoEnum Situacao { get; set; }
        public IFormFile? Imagem { get; set; }
        public int EmpresaId { get; set; }
    }
}
