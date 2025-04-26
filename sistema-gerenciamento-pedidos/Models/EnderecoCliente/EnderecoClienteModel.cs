using sistema_gerenciamento_pedidos.Models.Clientes;

namespace sistema_gerenciamento_pedidos.Models.EnderecoCliente
{
    public class EnderecoClienteModel
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }

        //Relacionamento entre as tabelas CLIENTE x ENDERECOCLIENTE
        public int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; }
    }
}
