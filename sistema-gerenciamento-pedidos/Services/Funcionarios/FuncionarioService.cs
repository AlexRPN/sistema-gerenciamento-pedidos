using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Models.Funcionarios;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;
using sistema_gerenciamento_pedidos.Services.Senha.Interface;

namespace sistema_gerenciamento_pedidos.Services.Funcionarios
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ISenhaService _senhaService;
        private readonly IMapper _mapper;

        public FuncionarioService(AppDbContext appDbContext,
                                  ISenhaService senhaService,
                                  IMapper mapper)
        {
            _appDbContext = appDbContext;
            _senhaService = senhaService;
            _mapper = mapper;
        }

        public async Task<ResponseModel<FuncionarioResponse>> Inserir(FuncionarioCriacaoDto funcionarioCriacaoDto)
        {
            ResponseModel<FuncionarioResponse> response = new ResponseModel<FuncionarioResponse>();

            try
            {
                var usuarioBanco = await _appDbContext.Funcionario.FirstOrDefaultAsync(x => x.NomeUsuario == funcionarioCriacaoDto.NomeUsuario && x.Telefone == funcionarioCriacaoDto.Telefone);

                if (usuarioBanco != null)
                {
                    response.Mensagem = "Telefone/Usuário já está cadastrado no sistema!";
                    return response;
                }

                _senhaService.CriarSenhaHash(funcionarioCriacaoDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                FuncionarioModel funcionario = _mapper.Map<FuncionarioModel>(funcionarioCriacaoDto);
                funcionario.SenhaHash = senhaHash;
                funcionario.SenhaSalt = senhaSalt;
                funcionario.Empresa = await _appDbContext.Empresa.FirstOrDefaultAsync(e => e.Id == funcionarioCriacaoDto.EmpresaId);

                var endereco = new EnderecoFuncionarioModel
                {
                    Logradouro = funcionarioCriacaoDto.Endereco.Logradouro,
                    Complemento = funcionarioCriacaoDto.Endereco.Complemento,
                    Cep = funcionarioCriacaoDto.Endereco.Cep,
                    FuncionarioId = funcionario.Id

                };

                funcionario.EnderecoFuncionario = _mapper.Map<EnderecoFuncionarioModel>(endereco);
                _appDbContext.Add(funcionario);
                await _appDbContext.SaveChangesAsync();

                var funcionarioResponse = _mapper.Map<FuncionarioResponse>(funcionario);

                response.Mensagem = "Funcionário cadastrado com sucesso";
                response.Dados = funcionarioResponse;

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
