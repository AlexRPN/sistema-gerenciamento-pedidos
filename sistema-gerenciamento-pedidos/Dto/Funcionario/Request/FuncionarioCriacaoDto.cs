using sistema_gerenciamento_pedidos.Enums;
using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_pedidos.Dto.Funcionario.Request
{
    public class FuncionarioCriacaoDto
    {
        [Required(ErrorMessage = "Digite o nome")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite o usuário")]
        public string NomeUsuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite a senha")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Digite a confirmação de senha"),
            Compare("Senha", ErrorMessage = "As senhas não são iguais!")]
        public string ConfirmaSenha { get; set; }
        [Required(ErrorMessage = "Digite o telefone")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Informe o perfil de acesso")]
        public PerfilAcessoEnum PerfilAcesso { get; set; }
        [Required(ErrorMessage = "Informe o Id da empresa a qual o funcionário será vinculado")]
        public int EmpresaId { get; set; }
    }
}
