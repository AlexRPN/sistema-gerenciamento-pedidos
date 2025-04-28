using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;

namespace sistema_gerenciamento_pedidos.Models.Clientes
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public SituacaoEnum Situacao { get; set; } = SituacaoEnum.Ativo;
        public PerfilAcessoEnum PerfilAcesso { get; set; } = PerfilAcessoEnum.cliente;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        //Relacionamento das tabelas Empresa x Cliente
        public int EmpresaId { get; set; }
        public EmpresaModel Empresa { get; set; }

        public EnderecoClienteModel EnderecoCliente { get; set; }
    }
}
