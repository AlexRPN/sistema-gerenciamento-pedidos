using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Services.PedidosJob.IPedidoJob;

namespace sistema_gerenciamento_pedidos.Services.PedidosJob
{
    public class PedidoJobService : IPedidoJobService
    {
        private readonly AppDbContext _appDbContext;

        public PedidoJobService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AtualizarStatusParaAguardandoEntrega(int pedidoId)
        {
            var pedido = await _appDbContext.Pedido.FindAsync(pedidoId);

            if (pedido != null && pedido.StatusPedido == StatusPedidoEnum.EmPreparacao)
            {
                pedido.StatusPedido = StatusPedidoEnum.AguardandoEntrega;
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
