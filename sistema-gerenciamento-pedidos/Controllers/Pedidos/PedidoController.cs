using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Pedido.Request;
using sistema_gerenciamento_pedidos.Dto.Pedido.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Pedidos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Cria novo pedido
        /// </summary>
        /// <param name="pedidoCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> CadastrarPedido(PedidoCriacaoDto pedidoCriacaoDto)
        {
            var pedido = await _pedidoService.Cadastrar(pedidoCriacaoDto);
            return Ok(pedido);
        }

        /// <summary>
        /// Lista os pedidos pelo status
        /// </summary>
        /// <param name="statusFiltro"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] StatusPedidoEnum? statusFiltro)
        {
            var resultado = await _pedidoService.Listar(statusFiltro);
            return Ok(resultado);
        }

        /// <summary>
        /// Busca pedido por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> BuscarPedidoPorId(int id)
        {
            var pedido = await _pedidoService.BuscarPedidoPorId(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Cancela o pedido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> CancelarPedido(int id)
        {
            var pedido = await _pedidoService.CancelarPedido(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Edita o pedido
        /// </summary>
        /// <param name="pedidoEdicao"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> EditarPedido(PedidoEdicaoDto pedidoEdicao)
        {
            var pedido = await _pedidoService.EditarPedido(pedidoEdicao);
            return Ok(pedido);
        }

        /// <summary>
        /// Altera o status do pedido
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <param name="novoStatus">Novo status</param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<ActionResult<ResponseModel<PedidoResponse>>> EditarStatusPedido([FromQuery] PedidoStatusDto dto)
        {
            var resultado = await _pedidoService.EditarStatusPedido(dto);
            return Ok(resultado);
        }


    }
}
