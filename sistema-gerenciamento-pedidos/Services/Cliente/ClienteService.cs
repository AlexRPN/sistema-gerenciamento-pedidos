using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
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
    }
}
