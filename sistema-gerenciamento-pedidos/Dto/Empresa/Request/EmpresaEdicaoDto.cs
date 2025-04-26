using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Empresa.Request
{
    public class EmpresaEdicaoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public string? Telefone { get; set; }
        public SituacaoEnum Situacao { get; set; }
    }
}
