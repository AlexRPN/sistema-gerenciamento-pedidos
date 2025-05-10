using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Services.EnderecoFuncionario.Interfaces;

namespace sistema_gerenciamento_pedidos.Services.EnderecoFuncionario
{
    public class EnderecoFuncionarioService : IEnderecoFuncionarioService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public EnderecoFuncionarioService(AppDbContext appDbContext,
                                          IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<EnderecoFuncionarioModel>> EditarEndereco(int idFuncionario, EnderecoFuncionarioEdicaoDto? enderecoFuncionarioEdicaoDto)
        {
            ResponseModel<EnderecoFuncionarioModel> response = new ResponseModel<EnderecoFuncionarioModel>();
            try
            {
                var enderecoFuncionario = await _appDbContext.EnderecoFuncionario
                    .FirstOrDefaultAsync(x => x.FuncionarioId == idFuncionario &&
                                              x.Id == enderecoFuncionarioEdicaoDto.Id);

                var enderecoEdicao = _mapper.Map<EnderecoFuncionarioEdicaoDto, EnderecoFuncionarioModel>(enderecoFuncionarioEdicaoDto, enderecoFuncionario);

                enderecoEdicao.FuncionarioId = idFuncionario;

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
