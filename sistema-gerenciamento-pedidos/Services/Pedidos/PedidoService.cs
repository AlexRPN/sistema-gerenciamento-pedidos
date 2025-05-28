using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Empresa.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Pedido.Request;
using sistema_gerenciamento_pedidos.Dto.Pedido.Response;
using sistema_gerenciamento_pedidos.Dto.PedidoProduto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.PedidoProduto;
using sistema_gerenciamento_pedidos.Models.Pedidos;
using sistema_gerenciamento_pedidos.Services.Pedidos.Interfaces;
using sistema_gerenciamento_pedidos.Services.PedidosJob;

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

        public async Task<ResponseModel<PedidoResponse>> BuscarPedidoPorId(int id)
        {
            ResponseModel<PedidoResponse> response = new ResponseModel<PedidoResponse>();

            try
            {
                var pedido = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                        .Include(p => p.Cliente.Empresa)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (pedido == null)
                {
                    response.Mensagem = "Pedido não localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                var pedidoResponse = new PedidoResponse
                {
                    Id = pedido.Id,
                    ValorTotal = pedido.ValorTotal,
                    DataPedido = pedido.DataPedido,
                    StatusPedido = pedido.StatusPedido,
                    TipoEntrega = pedido.TipoEntrega,
                    MotivoCancelamento = pedido.MotivoCancelamento,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = pedido.Cliente.Nome,
                        Telefone = pedido.Cliente.Telefone,
                        Endereco = pedido.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = pedido.Cliente.EnderecoCliente.Logradouro,
                                Complemento = pedido.Cliente.EnderecoCliente.Complemento,
                                Cep = pedido.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Empresa = pedido.Cliente.Empresa != null
                    ? new EmpresaResponse
                    {
                        Id = pedido.Cliente.Empresa.Id,
                        RazaoSocial = pedido.Cliente.Empresa.RazaoSocial,
                        Cnpj = pedido.Cliente.Empresa.Cnpj,
                        Telefone = pedido.Cliente.Empresa.Telefone,
                        Situacao = pedido.Cliente.Empresa.Situacao.ToString()
                    }
                    : null,
                    Produtos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        ValorUnitario = pp.ValorUnitario,
                        Observacao = pp.Observacao,
                        NomeProduto = pp.Produto != null ? pp.Produto.Nome : null
                    }).ToList()
                };

                response.Status = true;
                response.Mensagem = "Pedido localizado com sucesso!";
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
                    TipoPagamento = pedidoCriacaoDto.TipoPagamento,
                    TipoEntrega = pedidoCriacaoDto.TipoEntrega,
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

                BackgroundJob.Schedule<PedidoJobService>(
                    job => job.AtualizarStatusParaAguardandoEntrega(pedido.Id),
                    TimeSpan.FromMinutes(1)
                );

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

        public async Task<ResponseModel<PedidoResponse>> CancelarPedido(int id)
        {
            var response = new ResponseModel<PedidoResponse>();

            try
            {
                var pedido = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (pedido == null)
                {
                    response.Mensagem = "Pedido não localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                if (pedido.StatusPedido != StatusPedidoEnum.EmPreparacao)
                {
                    response.Mensagem = "Só é possível cancelar pedidos com status Em Preparação!";
                    response.Status = false;
                    return response;
                }

                pedido.StatusPedido = StatusPedidoEnum.Cancelado;

                _appDbContext.Pedido.Update(pedido);
                await _appDbContext.SaveChangesAsync();

                var pedidoResponse = new PedidoResponse
                {
                    Id = pedido.Id,
                    ValorTotal = pedido.ValorTotal,
                    DataPedido = pedido.DataPedido,
                    StatusPedido = pedido.StatusPedido,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = pedido.Cliente.Nome,
                        Telefone = pedido.Cliente.Telefone,
                        Endereco = pedido.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = pedido.Cliente.EnderecoCliente.Logradouro,
                                Complemento = pedido.Cliente.EnderecoCliente.Complemento,
                                Cep = pedido.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Produtos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        Observacao = pp.Observacao,
                        ValorUnitario = pp.ValorUnitario,
                        NomeProduto = pp.Produto?.Nome
                    }).ToList()
                };

                response.Status = true;
                response.Mensagem = "Pedido cancelado com sucesso!";
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

        public async Task<ResponseModel<PedidoResponse>> EditarStatusPedido(PedidoStatusDto dto)
        {
            var response = new ResponseModel<PedidoResponse>();

            try
            {
                var pedido = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(p => p.Id == dto.Id);

                if (pedido == null)
                {
                    response.Mensagem = "Pedido não localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                // Só permite cancelar se estiver em preparação
                if (dto.StatusPedido == StatusPedidoEnum.Cancelado && pedido.StatusPedido != StatusPedidoEnum.EmPreparacao)
                {
                    response.Mensagem = "Só é possível cancelar pedidos com status Em Preparação!";
                    response.Status = false;
                    return response;
                }

                pedido.StatusPedido = dto.StatusPedido;

                if (dto.StatusPedido == StatusPedidoEnum.Cancelado)
                    pedido.MotivoCancelamento = dto.MotivoCancelamento;

                _appDbContext.Pedido.Update(pedido);
                await _appDbContext.SaveChangesAsync();

                var pedidoResponse = new PedidoResponse
                {
                    Id = pedido.Id,
                    ValorTotal = pedido.ValorTotal,
                    DataPedido = pedido.DataPedido,
                    StatusPedido = pedido.StatusPedido,
                    MotivoCancelamento = pedido.MotivoCancelamento,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = pedido.Cliente.Nome,
                        Telefone = pedido.Cliente.Telefone,
                        Endereco = pedido.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = pedido.Cliente.EnderecoCliente.Logradouro,
                                Complemento = pedido.Cliente.EnderecoCliente.Complemento,
                                Cep = pedido.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Produtos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        Observacao = pp.Observacao,
                        ValorUnitario = pp.ValorUnitario,
                        NomeProduto = pp.Produto?.Nome
                    }).ToList()
                };
                response.Mensagem = "Status do pedido alterado com sucesso!";
                response.Dados = pedidoResponse;
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<PedidoResponse>> EditarPedido(PedidoEdicaoDto pedidoEdicaoDto)
        {
            var response = new ResponseModel<PedidoResponse>();

            try
            {
                // Carrega o pedido com seus relacionamentos
                var pedido = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(p => p.Id == pedidoEdicaoDto.Id);

                if (pedido == null)
                {
                    response.Mensagem = "Pedido não localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                if (pedido.StatusPedido != StatusPedidoEnum.EmPreparacao)
                {
                    response.Mensagem = "Só é possível editar pedidos com status Em Preparação!";
                    response.Status = false;
                    return response;
                }

                // Validação do novo cliente
                var clienteValido = await _appDbContext.Cliente
                    .Include(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(c => c.Id == pedidoEdicaoDto.ClienteId);

                if (clienteValido == null || clienteValido.Situacao != SituacaoEnum.Ativo)
                {
                    response.Mensagem = "Cliente inválido ou inativo!";
                    response.Status = false;
                    return response;
                }

                // Atualiza o cliente do pedido
                pedido.ClienteId = pedidoEdicaoDto.ClienteId;

                // Remove os produtos antigos
                _appDbContext.PedidoProduto.RemoveRange(pedido.PedidoProdutos);

                // Adiciona os novos produtos
                pedido.PedidoProdutos = pedidoEdicaoDto.PedidoProdutos.Select(produtoDto => new PedidoProdutoModel
                {
                    ProdutoId = produtoDto.ProdutoId,
                    Quantidade = produtoDto.Quantidade,
                    ValorUnitario = produtoDto.ValorUnitario,
                    Observacao = produtoDto.Observacao
                }).ToList();

                // Recalcula o valor total
                pedido.ValorTotal = pedido.PedidoProdutos.Sum(pp => pp.Quantidade * pp.ValorUnitario);

                _appDbContext.Pedido.Update(pedido);
                await _appDbContext.SaveChangesAsync();

                // Carrega novamente com os relacionamentos completos
                var pedidoAtualizado = await _appDbContext.Pedido
                    .Include(p => p.PedidoProdutos)
                        .ThenInclude(pp => pp.Produto)
                    .Include(p => p.Cliente)
                        .ThenInclude(c => c.EnderecoCliente)
                    .FirstOrDefaultAsync(p => p.Id == pedido.Id);

                var pedidoResponse = new PedidoResponse
                {
                    Id = pedidoAtualizado.Id,
                    ValorTotal = pedidoAtualizado.ValorTotal,
                    DataPedido = pedidoAtualizado.DataPedido,
                    StatusPedido = pedidoAtualizado.StatusPedido,
                    Cliente = new ClientePedidoResponse
                    {
                        Nome = pedidoAtualizado.Cliente.Nome,
                        Telefone = pedidoAtualizado.Cliente.Telefone,
                        Endereco = pedidoAtualizado.Cliente.EnderecoCliente != null
                            ? new EnderecoClienteResponse
                            {
                                Logradouro = pedidoAtualizado.Cliente.EnderecoCliente.Logradouro,
                                Complemento = pedidoAtualizado.Cliente.EnderecoCliente.Complemento,
                                Cep = pedidoAtualizado.Cliente.EnderecoCliente.Cep
                            }
                            : null
                    },
                    Produtos = pedidoAtualizado.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        ValorUnitario = pp.ValorUnitario,
                        Observacao = pp.Observacao,
                        NomeProduto = pp.Produto?.Nome
                    }).ToList()
                };

                response.Status = true;
                response.Mensagem = "Pedido atualizado com sucesso!";
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
