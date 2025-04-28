using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;

namespace sistema_gerenciamento_pedidos.Services.Cliente.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseModel<ClienteResponse>> Cadastrar(ClienteCriacaoDto clienteCriacaoDto);
    }
}
