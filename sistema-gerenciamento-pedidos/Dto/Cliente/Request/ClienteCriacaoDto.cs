using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Request;
using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Request
{
    public class ClienteCriacaoDto
    {
        [Required(ErrorMessage = "Digite o nome")]
        public string Nome { get; set; }
        //[Required(ErrorMessage = "Digite o telefone")]
        public TelefoneClienteCriacaoDto Telefone { get; set; }
        [Required(ErrorMessage = "Informe o Id da empresa que o cliente será cadastrado")]
        public int EmpresaId { get; set; }

        public EnderecoClienteCriacaoDto Endereco { get; set; }
    }
}
