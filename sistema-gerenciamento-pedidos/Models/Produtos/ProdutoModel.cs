using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.PedidoProduto;

namespace sistema_gerenciamento_pedidos.Models.Produtos
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public TamanhoEnum? Tamanho { get; set; }
        public SituacaoEnum Situacao { get; set; } = SituacaoEnum.Ativo;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string? Imagem { get; set; } = string.Empty;

        //Relacionamento entre as tabelas EMPRESA x PRODUTO
        public int EmpresaId { get; set; }
        public EmpresaModel Empresa { get; set; }

        public ICollection<PedidoProdutoModel> PedidoProdutos { get; set; }
    }
}
