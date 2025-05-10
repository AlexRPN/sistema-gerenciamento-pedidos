using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;

namespace sistema_gerenciamento_pedidos.Services.EnderecoFuncionario.Interfaces
{
    public interface IEnderecoFuncionarioService
    {
        Task<ResponseModel<EnderecoFuncionarioModel>> EditarEndereco(int idFuncionario, EnderecoFuncionarioEdicaoDto enderecoFuncionarioEdicaoDto);
    }
}
