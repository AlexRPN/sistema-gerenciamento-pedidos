using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Funcionarios
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        /// <summary>
        /// Busca funcionários por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioResponse>> BuscarPorId(int id)
        {
            var funcionario = await _funcionarioService.BuscarFuncionarioPorId(id);
            return Ok(funcionario);
        }

        /// <summary>
        /// Edita os dados do funcionário
        /// </summary>
        /// <param name="funcionarioEdicaoDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ResponseModel<FuncionarioResponse>>> Editar(FuncionarioEdicaoDto funcionarioEdicaoDto)
        {
            var funcionario = await _funcionarioService.Editar(funcionarioEdicaoDto, funcionarioEdicaoDto.Endereco);
            return Ok(funcionario);
        }

        /// <summary>
        /// Lista os funcionários
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        /// <param name="telefone"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<FuncionarioResponse>>> Listar(int? id, string? nome, string? telefone)
        {
            var funcionarios = await _funcionarioService.Listar(id, nome, telefone);
            return Ok(funcionarios);
        }

        /// <summary>
        /// Inativa o funcionário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<FuncionarioResponse>> Inativar(int id)
        {
            var funcionario = await _funcionarioService.Inativar(id);
            return Ok(funcionario);
        }
    }
}
