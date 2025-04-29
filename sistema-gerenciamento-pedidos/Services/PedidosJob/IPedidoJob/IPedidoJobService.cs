namespace sistema_gerenciamento_pedidos.Services.PedidosJob.IPedidoJob
{
    public interface IPedidoJobService
    {
        Task AtualizarStatusParaAguardandoEntrega(int pedidoId);
    }
}
