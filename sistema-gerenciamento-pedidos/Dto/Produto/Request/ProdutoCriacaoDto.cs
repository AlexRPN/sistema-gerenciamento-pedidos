using sistema_gerenciamento_pedidos.Enums;
using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.Produto.Request
{
    public class ProdutoCriacaoDto
    {
        [Required(ErrorMessage = "Informe o nome do produto")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe a descrição do produto")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Informe o valor do produto")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "Informe a categoria do produto")]
        public CategoriaEnum Categoria { get; set; }
        [Required(ErrorMessage = "Informe o tamanho do produto")]
        public TamanhoEnum Tamanho { get; set; }
        public IFormFile? Imagem { get; set; }
        [Required(ErrorMessage = "Informe o Id da empresa a qual o produto será vinculado")]
        public int EmpresaId { get; set; }
    }
}
