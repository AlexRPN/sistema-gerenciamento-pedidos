using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Services.EnderecoCliente.Interface;

namespace sistema_gerenciamento_pedidos.Services.EnderecoCliente
{
    public class EnderecoClienteService : IEnderecoClienteService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public EnderecoClienteService(AppDbContext appDbContext,
                                      IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<EnderecoClienteModel>> EditarEndereco(int idCliente, EnderecoClienteEdicaoDto enderecoClienteEdicaoDto)
        {
            ResponseModel<EnderecoClienteModel> response = new ResponseModel<EnderecoClienteModel>();

            try
            {
                var enderecoCliente = await _appDbContext.EnderecoCliente
                    .FirstOrDefaultAsync(x => x.ClienteId == idCliente && x.Id == enderecoClienteEdicaoDto.Id);

                var enderecoEdicao = _mapper.Map<EnderecoClienteEdicaoDto, EnderecoClienteModel>(enderecoClienteEdicaoDto, enderecoCliente);

                enderecoEdicao.ClienteId = idCliente;

                _appDbContext.Update(enderecoEdicao);
                await _appDbContext.SaveChangesAsync();

                response.Mensagem = "Endereço alterado com sucesso!";
                response.Dados = enderecoEdicao;

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
