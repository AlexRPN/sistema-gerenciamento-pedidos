using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Request;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Models.TelefoneClientes;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;
using sistema_gerenciamento_pedidos.Services.EnderecoCliente.Interface;

namespace sistema_gerenciamento_pedidos.Services.Cliente
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IEnderecoClienteService _enderecoClienteService;

        public ClienteService(AppDbContext appDbContext,
                              IMapper mapper,
                              IEnderecoClienteService enderecoClienteService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _enderecoClienteService = enderecoClienteService;
        }

        public async Task<ResponseModel<ClienteResponse>> BuscarClientePorId(int id)
        {
            ResponseModel<ClienteResponse> response = new ResponseModel<ClienteResponse>();

            try
            {
                var cliente = await _appDbContext.Cliente
                                                        .Include(c => c.TelefoneClientes)
                                                        .Include(c => c.EnderecoCliente)
                                                        .Include(c => c.Empresa)
                                                        .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não localizado no sistema!";
                    return response;
                }

                var clienteResponse = _mapper.Map<ClienteResponse>(cliente);

                response.Dados = clienteResponse;
                response.Mensagem = "Cliente localizado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ClienteResponse>> Cadastrar(ClienteCriacaoDto clienteCriacaoDto)
        {
            ResponseModel<ClienteResponse> response = new ResponseModel<ClienteResponse>();

            try
            {
                var clienteBanco = await _appDbContext.Cliente
                    .Include(c => c.TelefoneClientes)
                    .FirstOrDefaultAsync(tc => tc.TelefoneClientes.Telefone == clienteCriacaoDto.Telefone.Telefone);

                if (clienteBanco != null)
                {
                    response.Mensagem = "Telefone já está cadastrado no sistema!";
                    return response;
                }

                ClienteModel cliente = _mapper.Map<ClienteModel>(clienteCriacaoDto);

                cliente.Empresa = await _appDbContext.Empresa.FirstOrDefaultAsync(x => x.Id == clienteCriacaoDto.EmpresaId);

                var enderecoCliente = new EnderecoClienteModel
                {
                    Logradouro = clienteCriacaoDto.Endereco.Logradouro,
                    Complemento = clienteCriacaoDto.Endereco.Complemento,
                    Cep = clienteCriacaoDto.Endereco.Cep,
                    ClienteId = cliente.Id
                };

                var telefoneCliente = new TelefoneClienteDto
                {
                    Telefone = clienteCriacaoDto.Telefone.Telefone,
                    ClienteId = cliente.Id
                };

                cliente.EnderecoCliente = _mapper.Map<EnderecoClienteModel>(enderecoCliente);
                cliente.TelefoneClientes = _mapper.Map<TelefoneClienteDto>(telefoneCliente);

                cliente.DataCadastro = DateTime.Now;

                _appDbContext.Add(cliente);
                await _appDbContext.SaveChangesAsync();

                var clienteComTelefones = await _appDbContext.Cliente
                                                             .Include(c => c.TelefoneClientes)
                                                             .Include(c => c.EnderecoCliente)
                                                             .Include(c => c.Empresa)
                                                             .FirstOrDefaultAsync(c => c.Id == cliente.Id);

                ClienteResponse clienteResponse = _mapper.Map<ClienteResponse>(cliente);

                response.Mensagem = "Cliente cadastrado com sucesso.";
                response.Dados = clienteResponse;

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ClienteResponse>> Editar(ClienteEdicaoDto clienteEdicaoDto, EnderecoClienteEdicaoDto? enderecoClienteEdicaoDto)
        {
            var response = new ResponseModel<ClienteResponse>();

            try
            {
                var cliente = await _appDbContext.Cliente
                                                 .Include(c => c.EnderecoCliente)
                                                 .Include(c => c.TelefoneClientes)
                                                 .Include(c => c.Empresa)
                                                 .FirstOrDefaultAsync(c => c.Id == clienteEdicaoDto.Id);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não localizado no sistema!";
                    return response;
                }

                cliente.Nome = clienteEdicaoDto.Nome;
                cliente.TelefoneClientes.Telefone = clienteEdicaoDto.Telefone.Telefone;
                cliente.DataAlteracao = DateTime.Now;

                if (enderecoClienteEdicaoDto != null)
                {
                    var enderecoClienteResponse = await _enderecoClienteService.EditarEndereco(clienteEdicaoDto.Id, enderecoClienteEdicaoDto);

                    if (!enderecoClienteResponse.Status)
                    {
                        response.Mensagem = $"Erro ao editar endereço: {enderecoClienteResponse.Mensagem}";
                        response.Status = false;
                        return response;
                    }

                    var enderecoClienteBanco = _mapper.Map<EnderecoClienteModel>(enderecoClienteResponse.Dados);
                    cliente.EnderecoCliente = enderecoClienteBanco;
                }

                _appDbContext.Update(cliente);
                await _appDbContext.SaveChangesAsync();

                response.Mensagem = "Dados alterados com sucesso!";
                response.Dados = _mapper.Map<ClienteResponse>(cliente);

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ClienteResponse>> Inativar(int id)
        {
            var response = new ResponseModel<ClienteResponse>();

            try
            {
                var cliente = await _appDbContext.Cliente
                    .Include(c => c.TelefoneClientes)
                    .Include(c => c.EnderecoCliente)
                    .Include(c => c.Empresa)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não foi localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                cliente.Situacao = cliente.Situacao == SituacaoEnum.Ativo
                    ? SituacaoEnum.Inativo
                    : SituacaoEnum.Ativo;

                _appDbContext.Cliente.Update(cliente);
                await _appDbContext.SaveChangesAsync();

                response.Dados = _mapper.Map<ClienteResponse>(cliente);
                response.Mensagem = cliente.Situacao == SituacaoEnum.Ativo
                    ? "Cliente ativado com sucesso!"
                    : "Cliente inativado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ClienteResponse>>> Listar(int? id, string? nome, string? telefone)
        {
            ResponseModel<List<ClienteResponse>> response = new ResponseModel<List<ClienteResponse>>();

            try
            {
                var query = _appDbContext.Cliente
                    .Include(e => e.EnderecoCliente)
                    .Include(e => e.TelefoneClientes)
                    .Include(e => e.Empresa)
                    .AsQueryable();

                if (id.HasValue && id.Value > 0)
                    query = query.Where(c => c.Id == id.Value);

                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(c => c.Nome.Contains(nome));

                if (!string.IsNullOrWhiteSpace(telefone))
                    query = query.Where(c => c.TelefoneClientes != null && c.TelefoneClientes.Telefone == telefone);

                var clientes = await query.ToListAsync();

                if (!clientes.Any())
                {
                    response.Mensagem = "Nenhum cliente encontrado com os dados informados.";
                    return response;
                }

                var clientesResponse = _mapper.Map<List<ClienteResponse>>(clientes);

                response.Dados = clientesResponse;
                response.Mensagem = "Cliente localizado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = $"Erro ao listar clientes: {ex.Message}";
                response.Status = false;
                return response;
            }
        }

    }
}
