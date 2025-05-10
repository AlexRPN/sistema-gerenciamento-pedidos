using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Clientes
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Cadastra o cliente no sistema
        /// </summary>
        /// <param name="clienteCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> Cadastrar(ClienteCriacaoDto clienteCriacaoDto)
        {
            var cliente = await _clienteService.Cadastrar(clienteCriacaoDto);
            return Ok(cliente);
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ClienteResponse>>> Listar(int? id, string? nome, string? telefone)
        {
            var clientes = await _clienteService.Listar(id, nome, telefone);
            return Ok(clientes);
        }

        /// <summary>
        /// Edita os dados do cliente
        /// </summary>
        /// <param name="clienteEdicaoDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ResponseModel<ClienteResponse>>> Editar(ClienteEdicaoDto clienteEdicaoDto)
        {
            var cliente = await _clienteService.Editar(clienteEdicaoDto, clienteEdicaoDto.EnderecoCliente);
            return Ok(cliente);
        }

        /// <summary>
        /// Busca Cliente por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<ClienteResponse>>> BuscarClientePorId(int id)
        {
            var cliente = await _clienteService.BuscarClientePorId(id);
            return Ok(cliente);
        }

        /// <summary>
        /// Inativa o cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Inativar(int id)
        {
            var cliente = await _clienteService.Inativar(id);
            return Ok(cliente);
        }
    }
}
