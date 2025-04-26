using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Login.Request;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;

        public LoginController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        /// <summary>
        /// Faz o Login do usuário no sistema
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var usuario = await _funcionarioService.Login(loginDto);
            return Ok(usuario);
        }
    }
}
