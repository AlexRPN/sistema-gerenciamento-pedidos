using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Pedido.Request;
using sistema_gerenciamento_pedidos.Dto.Pedido.Response;
using sistema_gerenciamento_pedidos.Enums;

namespace sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces
{
    public interface IPedidoService
    {
        Task<ResponseModel<PedidoResponse>> Cadastrar(PedidoCriacaoDto pedidoCriacaoDto);
        Task<ResponseModel<List<PedidoResponse>>> Listar(StatusPedidoEnum? statusFiltro = null);
    }
}
