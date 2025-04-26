using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Dto.Cliente.Request
{
    public class ClienteEdicaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public SituacaoEnum Situacao { get; set; }
    }
}
