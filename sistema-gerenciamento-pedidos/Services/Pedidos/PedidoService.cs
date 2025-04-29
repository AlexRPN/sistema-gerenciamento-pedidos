using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Pedido.Request;
using sistema_gerenciamento_pedidos.Dto.Pedido.Response;
using sistema_gerenciamento_pedidos.Dto.PedidoProduto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.PedidoProduto;
using sistema_gerenciamento_pedidos.Models.Pedidos;
using sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces;

namespace sistema_gerenciamento_pedidos.Services.Pedidos
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public PedidoService(AppDbContext appDbContext,
                              IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<PedidoResponse>> Cadastrar(PedidoCriacaoDto pedidoCriacaoDto)
        {
            ResponseModel<PedidoResponse> response = new ResponseModel<PedidoResponse>();

            try
            {
                var clienteValido = await _appDbContext.Cliente
                                .FirstOrDefaultAsync(c => c.Id == pedidoCriacaoDto.ClienteId);

                if (clienteValido == null)
                {
                    response.Mensagem = "Cliente não encontrado.";
                    response.Status = false;
                    return response;
                }

                if (clienteValido.Situacao != SituacaoEnum.Ativo)
                {
                    response.Mensagem = "Cliente inativo. Não é possível gerar o pedido.";
                    response.Status = false;
                    return response;
                }

                // Preenchendo o PedidoModel
                PedidoModel pedido = new PedidoModel
                {
                    ClienteId = pedidoCriacaoDto.ClienteId,
                    DataPedido = DateTime.Now,
                    StatusPedido = StatusPedidoEnum.EmPreparacao,
                    PedidoProdutos = new List<PedidoProdutoModel>(),
                };

                if (pedidoCriacaoDto.PedidoProdutos != null && pedidoCriacaoDto.PedidoProdutos.Any())
                {
                    // Preenchendo a lista de produtos
                    foreach (var produtoDto in pedidoCriacaoDto.PedidoProdutos)
                    {
                        var produto = new PedidoProdutoModel
                        {
                            ProdutoId = produtoDto.ProdutoId,
                            Quantidade = produtoDto.Quantidade,
                            ValorUnitario = produtoDto.ValorUnitario,
                            Observacao = produtoDto.Observacao,
                        };

                        pedido.PedidoProdutos.Add(produto);
                    }

                    // Calculando o valor total
                    pedido.ValorTotal = pedido.PedidoProdutos.Sum(pp => pp.ValorUnitario * pp.Quantidade);
                }

                _appDbContext.Add(pedido);
                await _appDbContext.SaveChangesAsync();

                // Carregar o pedido com os produtos e os detalhes do produto
                var pedidoCompleto = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto) // Carregar a relação com o Produto
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(p => p.Id == pedido.Id);

                if (pedidoCompleto == null)
                {
                    response.Mensagem = "Erro ao carregar o pedido.";
                    response.Status = false;
                    return response;
                }

                // Preenchendo manualmente o PedidoResponse
                PedidoResponse pedidoResponse = new PedidoResponse
                {
                    Id = pedidoCompleto.Id,
                    ValorTotal = pedidoCompleto.ValorTotal,
                    DataPedido = pedidoCompleto.DataPedido,
                    StatusPedido = pedidoCompleto.StatusPedido,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = pedidoCompleto.Cliente.Nome,
                        Telefone = pedidoCompleto.Cliente.Telefone,
                        // Verificando se o EnderecoCliente é null
                        Endereco = pedidoCompleto.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = pedidoCompleto.Cliente.EnderecoCliente.Logradouro,
                                Complemento = pedidoCompleto.Cliente.EnderecoCliente.Complemento,
                                Cep = pedidoCompleto.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Produtos = pedidoCompleto.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        Observacao = pp.Observacao,
                        ValorUnitario = pp.ValorUnitario,
                        // Agora o nome do produto será preenchido corretamente
                        NomeProduto = pp.Produto != null ? pp.Produto.Nome : null
                    }).ToList(),
                };

                response.Mensagem = "Pedido cadastrado com sucesso.";
                response.Dados = pedidoResponse;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<List<PedidoResponse>>> Listar(StatusPedidoEnum? statusFiltro = null)
        {
            ResponseModel<List<PedidoResponse>> response = new ResponseModel<List<PedidoResponse>>();

            try
            {
                var query = _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .AsQueryable();

                if (statusFiltro.HasValue)
                {
                    query = query.Where(p => p.StatusPedido == statusFiltro.Value);
                }

                var pedidos = await query.ToListAsync();

                if (!pedidos.Any())
                {
                    response.Mensagem = "Nenhum pedido encontrado com os critérios informados.";
                    return response;
                }

                var pedidosResponse = pedidos.Select(p => new PedidoResponse
                {
                    Id = p.Id,
                    ValorTotal = p.ValorTotal,
                    DataPedido = p.DataPedido,
                    StatusPedido = p.StatusPedido,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = p.Cliente.Nome,
                        Telefone = p.Cliente.Telefone,
                        Endereco = p.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = p.Cliente.EnderecoCliente.Logradouro,
                                Complemento = p.Cliente.EnderecoCliente.Complemento,
                                Cep = p.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Produtos = p.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        Observacao = pp.Observacao,
                        ValorUnitario = pp.ValorUnitario,
                        NomeProduto = pp.Produto != null ? pp.Produto.Nome : null
                    }).ToList()
                }).ToList();

                response.Status = true;
                response.Dados = pedidosResponse;
                response.Mensagem = "Pedidos listados com sucesso.";

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
