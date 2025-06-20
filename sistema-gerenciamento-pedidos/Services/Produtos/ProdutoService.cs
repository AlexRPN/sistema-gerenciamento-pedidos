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

        public ProdutoService(AppDbContext appDbContext,
                              IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
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
                var produtoBanco = await _appDbContext.Produto.FirstOrDefaultAsync(
                    x => x.Descricao == produtoCriacaoDto.Descricao &&
                         x.Tamanho == produtoCriacaoDto.Tamanho &&
                         x.Nome == produtoCriacaoDto.Nome);

                if (produtoBanco != null)
                {
                    response.Mensagem = "O Produto já está cadastrado no sistema!";
                    return response;
                }

                ProdutoModel produto = _mapper.Map<ProdutoModel>(produtoCriacaoDto);

                // Salvar imagem no servidor
                if (produtoCriacaoDto.Imagem != null && produtoCriacaoDto.Imagem.Length > 0)
                {
                    var nomeArquivo = $"{Guid.NewGuid()}{Path.GetExtension(produtoCriacaoDto.Imagem.FileName)}";
                    var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

                    if (!Directory.Exists(caminhoPasta))
                        Directory.CreateDirectory(caminhoPasta);

                    var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await produtoCriacaoDto.Imagem.CopyToAsync(stream);
                    }

                    produto.Imagem = $"imagens/{nomeArquivo}";
                }

                produto.Tamanho ??= TamanhoEnum.NaoInformado;
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

        public async Task<ResponseModel<ProdutoResponse>> Editar(ProdutoEdicaoDto produtoEdicaoDto, IFormFile? imagem)
        {
            ResponseModel<ProdutoResponse> response = new ResponseModel<ProdutoResponse>();

            try
            {
                var produtoBanco = await _appDbContext.Produto.FirstOrDefaultAsync(x => x.Id == produtoEdicaoDto.Id);

                if (produtoBanco == null)
                {
                    response.Mensagem = "Produto não encontrado.";
                    response.Status = false;
                    return response;
                }

                _mapper.Map(produtoEdicaoDto, produtoBanco);
                produtoBanco.DataAlteracao = DateTime.Now;
                produtoBanco.Tamanho ??= TamanhoEnum.NaoInformado;

                // Se houver nova imagem, salvar e atualizar o nome
                if (imagem != null && imagem.Length > 0)
                {
                    // Salva nova imagem
                    var pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

                    if (!Directory.Exists(pastaDestino))
                        Directory.CreateDirectory(pastaDestino);

                    var nomeImagem = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
                    var caminhoCompleto = Path.Combine(pastaDestino, nomeImagem);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    produtoBanco.Imagem = $"imagens/{nomeImagem}";
                }
                else
                {
                    // Mantém a imagem anterior (enviada como string no DTO)
                    produtoBanco.Imagem = produtoEdicaoDto.Imagem;
                }

                _appDbContext.Produto.Update(produtoBanco);
                await _appDbContext.SaveChangesAsync();

                ProdutoResponse produtoResponse = _mapper.Map<ProdutoResponse>(produtoBanco);

                response.Mensagem = "Dados alterados com sucesso!";
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

        public async Task<ResponseModel<List<ProdutoResponse>>> Listar(CategoriaEnum? categoria)
        {
            ResponseModel<List<ProdutoResponse>> response = new ResponseModel<List<ProdutoResponse>>();

            try
            {
                var produtosQuery = _appDbContext.Produto.AsQueryable();

                if (categoria.HasValue)
                {
                    produtosQuery = produtosQuery.Where(p => p.Categoria == categoria.Value);
                }

                var produtos = produtosQuery.ToList();

                if (!produtos.Any())
                {
                    response.Mensagem = "Nenhum produto encontrado para a categoria especificada.";
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
