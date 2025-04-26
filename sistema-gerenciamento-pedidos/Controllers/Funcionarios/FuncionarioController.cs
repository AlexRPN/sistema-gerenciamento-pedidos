using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Funcionarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        /// <summary>
        /// Cria um funcionário
        /// </summary>
        /// <param name="funcionarioCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FuncionarioResponse>> Inserir(FuncionarioCriacaoDto funcionarioCriacaoDto)
        {
            var funcionario = await _funcionarioService.Inserir(funcionarioCriacaoDto);
            return Ok(funcionario);
        }
    }
}
