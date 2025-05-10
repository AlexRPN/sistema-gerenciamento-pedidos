using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_pedidos.Data;
using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Dto.Login.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;
using sistema_gerenciamento_pedidos.Models.Funcionarios;
using sistema_gerenciamento_pedidos.Services.EnderecoFuncionario.Interfaces;
using sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces;
using sistema_gerenciamento_pedidos.Services.Senha.Interface;

namespace sistema_gerenciamento_pedidos.Services.Funcionarios
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ISenhaService _senhaService;
        private readonly IMapper _mapper;
        private readonly IEnderecoFuncionarioService _enderecoFuncionarioService;

        public FuncionarioService(AppDbContext appDbContext,
                                  ISenhaService senhaService,
                                  IMapper mapper,
                                  IEnderecoFuncionarioService enderecoFuncionarioService)
        {
            _appDbContext = appDbContext;
            _senhaService = senhaService;
            _mapper = mapper;
            _enderecoFuncionarioService = enderecoFuncionarioService;
        }

        public async Task<ResponseModel<FuncionarioResponse>> BuscarFuncionarioPorId(int id)
        {
            ResponseModel<FuncionarioResponse> response = new ResponseModel<FuncionarioResponse>();

            try
            {
                var funcionario = await _appDbContext.Funcionario.Include(x => x.EnderecoFuncionario).Include(x => x.Empresa).FirstOrDefaultAsync(x => x.Id == id);

                if (funcionario == null)
                {
                    response.Mensagem = "Funcionário não localizado no sistema!";
                    return response;
                }

                var funcionarioResponse = _mapper.Map<FuncionarioResponse>(funcionario);

                response.Dados = funcionarioResponse;
                response.Mensagem = "Funcionário localizado com sucesso!";

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<FuncionarioResponse>> Editar(FuncionarioEdicaoDto funcionarioEdicaoDto, EnderecoFuncionarioEdicaoDto? enderecoFuncionarioEdicaoDto)
        {
            ResponseModel<FuncionarioResponse> response = new ResponseModel<FuncionarioResponse>();
            try
            {
                var funcionario = await _appDbContext.Funcionario
                                                     .Include(x => x.EnderecoFuncionario)
                                                     .Include(x => x.Empresa)
                                                     .FirstOrDefaultAsync(x => x.Id == funcionarioEdicaoDto.Id);

                if (funcionario == null)
                {
                    response.Mensagem = "Funcionário não localizado no sistema!";
                    return response;
                }

                funcionario.Nome = funcionarioEdicaoDto.Nome;
                funcionario.Telefone = funcionarioEdicaoDto.Telefone;
                funcionario.PerfilAcesso = (Enums.PerfilAcessoEnum)funcionarioEdicaoDto.PerfilAcesso;
                funcionario.DataAlteracao = DateTime.Now;

                if (enderecoFuncionarioEdicaoDto != null)
                {
                    var enderecoFuncionarioResponse = await _enderecoFuncionarioService
                        .EditarEndereco(funcionarioEdicaoDto.Id, enderecoFuncionarioEdicaoDto);

                    if (!enderecoFuncionarioResponse.Status)
                    {
                        response.Mensagem = $"Erro ao editar endereço: {enderecoFuncionarioResponse.Mensagem}";
                        response.Status = false;
                        return response;
                    }

                    var enderecoFuncionarioBanco = _mapper.Map<EnderecoFuncionarioModel>(enderecoFuncionarioResponse.Dados);

                    funcionario.EnderecoFuncionario = enderecoFuncionarioBanco;
                }

                _appDbContext.Update(funcionario);
                await _appDbContext.SaveChangesAsync();

                var funcionarioResponse = _mapper.Map<FuncionarioResponse>(funcionario);

                response.Mensagem = "Dados alterados com sucesso!";
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

        public async Task<ResponseModel<FuncionarioResponse>> Inativar(int id)
        {
            var response = new ResponseModel<FuncionarioResponse>();

            try
            {
                var funcionario = await _appDbContext.Funcionario.Include(x => x.EnderecoFuncionario).Include(x => x.Empresa).FirstOrDefaultAsync(x => x.Id == id);

                if (funcionario == null)
                {
                    response.Mensagem = "Funcionário não foi localizado no sistema!";
                    response.Status = false;
                    return response;
                }

                funcionario.Situacao = funcionario.Situacao == SituacaoEnum.Ativo
                    ? SituacaoEnum.Inativo
                    : SituacaoEnum.Ativo;
                funcionario.DataAlteracao = DateTime.Now;

                _appDbContext.Funcionario.Update(funcionario);
                await _appDbContext.SaveChangesAsync();

                var funcionarioResponse = _mapper.Map<FuncionarioResponse>(funcionario);

                response.Dados = funcionarioResponse;
                response.Mensagem = funcionario.Situacao == SituacaoEnum.Ativo
                    ? "funcionario ativado com sucesso!"
                    : "funcionario inativado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
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

        public async Task<ResponseModel<List<FuncionarioResponse>>> Listar(int? id, string? nome, string? telefone)
        {
            ResponseModel<List<FuncionarioResponse>> response = new ResponseModel<List<FuncionarioResponse>>();

            try
            {
                var query = _appDbContext.Funcionario
                    .Include(x => x.EnderecoFuncionario)
                    .Include(x => x.Empresa)
                    .AsQueryable();

                if (id.HasValue && id.Value > 0)
                    query = query.Where(x => x.Id == id);

                if (!string.IsNullOrEmpty(nome))
                    query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));

                if (!string.IsNullOrEmpty(telefone))
                    query = query.Where(x => x.Telefone.Contains(telefone));

                var funcionarios = await query.ToListAsync();

                if (!funcionarios.Any())
                {
                    response.Mensagem = "Nenhum funcionário encontrado com os dados informados!";
                    return response;
                }

                var funcionariosResponse = _mapper.Map<List<FuncionarioResponse>>(funcionarios);

                response.Dados = funcionariosResponse;
                response.Mensagem = "Funcionários localizados com sucesso!";

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<FuncionarioModel>> Login(LoginDto loginDto)
        {
            ResponseModel<FuncionarioModel> response = new ResponseModel<FuncionarioModel>();

            try
            {
                var funcionario = await _appDbContext.Funcionario.FirstOrDefaultAsync(x => x.NomeUsuario == loginDto.NomeUsuario);

                if (funcionario == null)
                {
                    response.Mensagem = "Usuário não localizado!";
                    response.Status = false;

                    return response;
                }

                if (!_senhaService.VerificaSenhaHash(loginDto.Senha, funcionario.SenhaHash, funcionario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais inválidas!";
                    response.Status = false;

                    return response;
                }

                var token = _senhaService.CriarToken(funcionario);

                funcionario.Token = token;

                _appDbContext.Update(funcionario);
                await _appDbContext.SaveChangesAsync();

                response.Dados = funcionario;
                response.Mensagem = "Usuário logado com sucesso!";

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
