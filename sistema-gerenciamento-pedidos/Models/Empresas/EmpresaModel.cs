using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Models.Empresas
{
    public class EmpresaModel
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public SituacaoEnum Situacao { get; set; } = SituacaoEnum.Ativo;
    }
}
