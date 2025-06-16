using sistema_gerenciamento_pedidos.Models.Clientes;

namespace sistema_gerenciamento_pedidos.Models.TelefoneClientes
{
    public class TelefoneClienteDto
    {
        public int Id { get; set; }
        public string Telefone { get; set; }

        public int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; }
    }
}
