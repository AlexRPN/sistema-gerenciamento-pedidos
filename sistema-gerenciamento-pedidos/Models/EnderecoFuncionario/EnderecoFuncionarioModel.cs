using sistema_gerenciamento_pedidos.Models.Funcionarios;

namespace sistema_gerenciamento_pedidos.Models.EnderecoFuncionario
{
    public class EnderecoFuncionarioModel
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }

        //Relacionamento de tabelas FUNCIONARIO x ENDERECOFUNCIONARIO
        public int FuncionarioId { get; set; }
        public FuncionarioModel Funcionario { get; set; }
    }
}
