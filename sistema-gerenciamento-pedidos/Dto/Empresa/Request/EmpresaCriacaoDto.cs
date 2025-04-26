using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.Empresa.Request
{
    public class EmpresaCriacaoDto
    {
        [Required(ErrorMessage = "Digite a razão social da empresa")]
        public string RazãoSocial { get; set; }
        [Required(ErrorMessage = "Digite o CNPJ da empresa")]
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "Digite o telefone da empresa")]
        public string Telefone { get; set; }
    }
}
