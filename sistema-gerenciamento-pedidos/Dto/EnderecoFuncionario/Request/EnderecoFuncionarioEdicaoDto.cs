namespace sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request
{
    public class EnderecoFuncionarioEdicaoDto
    {
        public int Id { get; set; }
        public string? Logradouro { get; set; }
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
    }
}
