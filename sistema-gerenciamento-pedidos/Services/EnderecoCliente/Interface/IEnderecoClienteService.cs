using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;

namespace sistema_gerenciamento_pedidos.Services.EnderecoCliente.Interface
{
    public interface IEnderecoClienteService
    {
        Task<ResponseModel<EnderecoClienteModel>> EditarEndereco(int idCliente, EnderecoClienteEdicaoDto enderecoClienteEdicaoDto);
    }
}
