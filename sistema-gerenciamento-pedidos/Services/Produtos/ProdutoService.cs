using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Produto.Request;
using sistema_gerenciamento_pedidos.Dto.Produto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Produtos;
using sistema_gerenciamento_pedidos.Services.Produtos.Interfaces;

namespace sistema_gerenciamento_pedidos.Services.Produtos
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private string _caminhoServidor;

        public ProdutoService(AppDbContext appDbContext,
                              IMapper mapper,
                              IWebHostEnvironment sistema)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _caminhoServidor = sistema.WebRootPath;
        }

        public async Task<ResponseModel<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            ResponseModel<ProdutoResponse> response = new ResponseModel<ProdutoResponse>();

            try
            {
                var produto = await _appDbContext.Produto.FindAsync(id);

                if (produto == null)
                {
                    response.Mensagem = "Produto não está cadastrado no sistema!";
                    return response;
                }

                var produtoLocalizado = _mapper.Map<ProdutoResponse>(produto);

                response.Dados = produtoLocalizado;
                response.Mensagem = "Produto localizado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ProdutoResponse>> Cadastrar(ProdutoCriacaoDto produtoCriacaoDto)
        {

            ResponseModel<ProdutoResponse> response = new ResponseModel<ProdutoResponse>();

            try
            {
                var produtoBanco = await _appDbContext.Produto.FirstOrDefaultAsync(x => x.Descricao == produtoCriacaoDto.Descricao && x.Imagem == produtoCriacaoDto.Imagem);

                if (produtoBanco != null)
                {
                    response.Mensagem = "O Produto já está cadastrado no sistema!";
                    return response;
                }

                ProdutoModel produto = _mapper.Map<ProdutoModel>(produtoCriacaoDto);

                produto.Empresa = await _appDbContext.Empresa.FirstOrDefaultAsync(e => e.Id == produtoCriacaoDto.EmpresaId);
                produto.DataCadastro = DateTime.Now;

                _appDbContext.Add(produto);
                await _appDbContext.SaveChangesAsync();

                ProdutoResponse produtoResponse = _mapper.Map<ProdutoResponse>(produto);

                response.Mensagem = "Produto cadastrado com sucesso.";
                response.Dados = produtoResponse;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ProdutoModel>> Editar(ProdutoEdicaoDto produtoEdicaoDto)
        {
            ResponseModel<ProdutoModel> response = new ResponseModel<ProdutoModel>();

            try
            {
                var produto = await _appDbContext.Produto.AsNoTracking().FirstOrDefaultAsync(x => x.Id == produtoEdicaoDto.Id);

                var produtoEdicao = _mapper.Map<ProdutoModel>(produtoEdicaoDto);

                produtoEdicao.DataAlteracao = DateTime.Now;

                _appDbContext.Update(produtoEdicao);
                await _appDbContext.SaveChangesAsync();

                response.Mensagem = "Dados alterados com sucesso!";
                response.Dados = produtoEdicao;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ProdutoModel>> Inativar(int id)
        {
            var response = new ResponseModel<ProdutoModel>();

            try
            {
                var produto = await _appDbContext.Produto.FindAsync(id);

                if (produto == null)
                {
                    response.Mensagem = "Produto não localizado no sistema!";
                    response.Status = false;

                    return response;
                }

                produto.Situacao = produto.Situacao == SituacaoEnum.Ativo
                    ? SituacaoEnum.Inativo
                    : SituacaoEnum.Ativo;

                _appDbContext.Produto.Update(produto);
                await _appDbContext.SaveChangesAsync();

                response.Dados = produto;
                response.Mensagem = produto.Situacao == SituacaoEnum.Ativo
                    ? "Produto ativado com sucesso!"
                    : "Produto inativado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<List<ProdutoResponse>>> Listar()
        {
            ResponseModel<List<ProdutoResponse>> response = new ResponseModel<List<ProdutoResponse>>();

            try
            {
                var produtos = _appDbContext.Produto.ToList();

                if (produtos.Count() == 0)
                {
                    response.Mensagem = "Nenhum produto cadastrado!";
                    return response;
                }

                var produtoResponse = _mapper.Map<List<ProdutoResponse>>(produtos);

                response.Dados = produtoResponse;
                response.Mensagem = "Produtos localizados com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }
    }
}
