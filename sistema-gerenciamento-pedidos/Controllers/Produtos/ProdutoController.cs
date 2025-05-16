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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProdutoResponse>> Cadastrar([FromForm] ProdutoCriacaoDto produtoCriacaoDto)
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
        /// <param name="imagem"></param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProdutoResponse>> Editar([FromForm] ProdutoEdicaoDto produtoEdicaoDto, IFormFile? imagem)
        {
            var produto = await _produtoService.Editar(produtoEdicaoDto, imagem);
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
