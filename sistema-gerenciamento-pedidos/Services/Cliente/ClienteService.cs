using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Services.Cliente.Interfaces;

namespace sistema_gerenciamento_pedidos.Services.Cliente
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ClienteService(AppDbContext appDbContext,
                              IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<ClienteResponse>> BuscarClientePorId(int id)
        {
            ResponseModel<ClienteResponse> response = new ResponseModel<ClienteResponse>();

            try
            {
                var cliente = await _appDbContext.Cliente.FindAsync(id);

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
                var clienteBanco = await _appDbContext.Cliente.FirstOrDefaultAsync(x => x.Telefone == clienteCriacaoDto.Telefone);

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

                cliente.EnderecoCliente = _mapper.Map<EnderecoClienteModel>(enderecoCliente);
                cliente.DataCadastro = DateTime.Now;


                _appDbContext.Add(cliente);
                await _appDbContext.SaveChangesAsync();

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

        public async Task<ResponseModel<ClienteModel>> Editar(ClienteEdicaoDto clienteEdicaoDto)
        {
            ResponseModel<ClienteModel> response = new ResponseModel<ClienteModel>();

            try
            {
                var cliente = await _appDbContext.Cliente.FindAsync(clienteEdicaoDto.Id);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não localizado no sistema!";
                    return response;
                }

                cliente.Nome = clienteEdicaoDto.Nome;
                cliente.Telefone = clienteEdicaoDto.Telefone;
                cliente.DataAlteracao = DateTime.Now;

                _appDbContext.Update(cliente);
                await _appDbContext.SaveChangesAsync();

                response.Mensagem = "Dados alterados com sucesso!";
                response.Dados = cliente;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<ClienteModel>> Inativar(int id)
        {

            var response = new ResponseModel<ClienteModel>();

            try
            {
                var cliente = await _appDbContext.Cliente.FindAsync(id);

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

                response.Dados = cliente;
                response.Mensagem = cliente.Situacao == SituacaoEnum.Ativo
                    ? "Cliente ativada com sucesso!"
                    : "Cliente inativada com sucesso!";

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
                    .Include(e => e.Empresa)
                    .AsQueryable();

                if (id.HasValue && id.Value > 0)
                    query = query.Where(c => c.Id == id.Value);

                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(c => c.Nome.Contains(nome));

                if (!string.IsNullOrWhiteSpace(telefone))
                    query = query.Where(c => c.Telefone.Contains(telefone));

                var clientes = await query.ToListAsync();

                if (!clientes.Any())
                {
                    response.Mensagem = "Nenhum cliente encontrado com os dados informados.";
                    return response;
                }

                var clientesResponse = _mapper.Map<List<ClienteResponse>>(clientes);

                response.Dados = clientesResponse;
                response.Mensagem = "Cliente localizado com sucesso!";
                response.Status = true;

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
