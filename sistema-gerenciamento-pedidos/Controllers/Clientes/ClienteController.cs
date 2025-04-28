using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Clientes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
    }
}
