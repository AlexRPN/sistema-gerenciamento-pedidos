using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;

namespace sistema_gerenciamento_pedidos.Models.Funcionarios
{
    public class FuncionarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public byte[] SenhaHash { get; set; } = new byte[0];
        public byte[] SenhaSalt { get; set; } = new byte[0];
        public string Telefone { get; set; } = string.Empty;
        public SituacaoEnum Situacao { get; set; } = SituacaoEnum.Ativo;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; }
        public PerfilAcessoEnum PerfilAcesso { get; set; }

        //Relacionamento de tabelas EMPRESA x FUNCIONARIO
        public int EmpresaId { get; set; }
        public EmpresaModel Empresa { get; set; }

        public EnderecoFuncionarioModel EnderecoFuncionario { get; set; }
    }
}
