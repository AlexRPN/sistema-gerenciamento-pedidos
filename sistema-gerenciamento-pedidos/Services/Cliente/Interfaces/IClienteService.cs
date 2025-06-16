using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.Clientes;

namespace sistema_gerenciamento_pedidos.Services.Cliente.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseModel<ClienteResponse>> Cadastrar(ClienteCriacaoDto clienteCriacaoDto);
        Task<ResponseModel<List<ClienteResponse>>> Listar(int? id, string? nome, string? telefone);
        Task<ResponseModel<ClienteResponse>> Editar(ClienteEdicaoDto clienteEdicaoDto, EnderecoClienteEdicaoDto? enderecoClienteEdicaoDto);
        Task<ResponseModel<ClienteResponse>> BuscarClientePorId(int id);
        Task<ResponseModel<ClienteResponse>> Inativar(int id);
    }
}
