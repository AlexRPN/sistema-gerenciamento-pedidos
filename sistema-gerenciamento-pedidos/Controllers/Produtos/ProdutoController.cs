using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_pedidos.Dto.Produto.Request;
using sistema_gerenciamento_pedidos.Dto.Produto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Services.Produtos.Interfaces;

namespace sistema_gerenciamento_pedidos.Controllers.Produtos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        /// <summary>
        /// Cria um novo produto no sistema
        /// </summary>
        /// <param name="produtoCriacaoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> Cadastrar(ProdutoCriacaoDto produtoCriacaoDto)
        {
            var produto = await _produtoService.Cadastrar(produtoCriacaoDto);
            return Ok(produto);
        }

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ProdutoResponse>>> Listar(CategoriaEnum? categoria)
        {
            var produtos = await _produtoService.Listar(categoria);
            return Ok(produtos);
        }

        /// <summary>
        /// Edita o produto
        /// </summary>
        /// <param name="produtoEdicaoDto"></param>
        /// <param name="foto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ProdutoResponse>> Editar(ProdutoEdicaoDto produtoEdicaoDto)
        {
            var produto = await _produtoService.Editar(produtoEdicaoDto);
            return Ok(produto);
        }

        /// <summary>
        /// Busca produto por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarProdutoPorId(int id)
        {
            var produto = await _produtoService.BuscarProdutoPorId(id);
            return Ok(produto);
        }

        /// <summary>
        /// Inativa produto por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> InativarProduto(int id)
        {
            var produto = await _produtoService.Inativar(id);
            return Ok(produto);
        }
    }
}
