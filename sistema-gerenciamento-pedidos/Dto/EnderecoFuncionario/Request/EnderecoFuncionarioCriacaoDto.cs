using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request
{
    public class EnderecoFuncionarioCriacaoDto
    {
        [Required(ErrorMessage = "Digite o logradouro")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "Digite o complemento")]
        public string Complemento { get; set; }
        [Required(ErrorMessage = "Digite o CEP")]
        public string Cep { get; set; }
    }
}
