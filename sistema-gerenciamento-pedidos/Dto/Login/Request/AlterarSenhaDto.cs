using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.Login.Request
{
    public class AlterarSenhaDto
    {
        [Required(ErrorMessage = "O campo Nome de Usuário é obrigatório.")]
        public string NomeUsuario { get; set; }
        [Required(ErrorMessage = "O campo Senha Atual é obrigatório.")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = "O campo Nova Senha é obrigatório.")]
        public string NovaSenha { get; set; }
        [Required(ErrorMessage = "O campo Confirmação de Nova Senha é obrigatório.")]
        public string ConfirmaNovaSenha { get; set; }
    }
}
