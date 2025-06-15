using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Pedido.Request;
using sistema_gerenciamento_pedidos.Dto.Pedido.Response;
using sistema_gerenciamento_pedidos.Dto.Produto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;
using sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces;
using sistema_gerenciamento_pedidos.Services.Produtos.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Clientes.PedidoClientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoClientesController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IClienteService _clienteService;
        private readonly IPedidoService _pedidoService;

        public PedidoClientesController(IProdutoService produtoService,
                                        IClienteService clienteService,
                                        IPedidoService pedidoService)
        {
            _produtoService = produtoService;
            _clienteService = clienteService;
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Cadastra o cliente no sistema
        /// </summary>
        /// <param name="clienteCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost("cliente")]
        public async Task<ActionResult<ClienteResponse>> CadastrarCliente(ClienteCriacaoDto clienteCriacaoDto)
        {
            var cliente = await _clienteService.Cadastrar(clienteCriacaoDto);
            return Ok(cliente);
        }

        /// <summary>
        /// Edita os dados do cliente
        /// </summary>
        /// <param name="clienteEdicaoDto"></param>
        /// <returns></returns>
        [HttpPut("cliente")]
        public async Task<ActionResult<ResponseModel<ClienteResponse>>> EditarCliente(ClienteEdicaoDto clienteEdicaoDto)
        {
            var cliente = await _clienteService.Editar(clienteEdicaoDto, clienteEdicaoDto.EnderecoCliente);
            return Ok(cliente);
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet("clientes")]
        public async Task<ActionResult<List<ClienteResponse>>> ListarCliente(int? id, string? nome, string? telefone)
        {
            var clientes = await _clienteService.Listar(id, nome, telefone);
            return Ok(clientes);
        }

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns></returns>
        [HttpGet("produtos")]
        public async Task<ActionResult<List<ProdutoResponse>>> ListarProdutos(CategoriaEnum? categoria)
        {
            var produtos = await _produtoService.Listar(categoria);
            return Ok(produtos);
        }

        /// <summary>
        /// Busca pedido por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("pedido/{id}")]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> BuscarPedidoId(int id)
        {
            var pedido = await _pedidoService.BuscarPedidoPorId(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Cria novo pedido
        /// </summary>
        /// <param name="pedidoCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost("pedido")]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> CadastrarNovoPedido(PedidoCriacaoDto pedidoCriacaoDto)
        {
            var pedido = await _pedidoService.Cadastrar(pedidoCriacaoDto);
            return Ok(pedido);
        }
    }
}
