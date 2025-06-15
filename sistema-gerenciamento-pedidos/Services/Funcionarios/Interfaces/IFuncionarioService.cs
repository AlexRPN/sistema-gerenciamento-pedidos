using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Dto.Login.Request;
using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Models.Funcionarios;

namespace sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces
{
    public interface IFuncionarioService
    {
        Task<ResponseModel<FuncionarioResponse>> Inserir(FuncionarioCriacaoDto funcionarioCriacaoDto);
        Task<ResponseModel<FuncionarioResponse>> Editar(FuncionarioEdicaoDto funcionarioEdicaoDto, EnderecoFuncionarioEdicaoDto? enderecoFuncionarioEdicaoDto);
        Task<ResponseModel<FuncionarioResponse>> BuscarFuncionarioPorId(int id);
        Task<ResponseModel<List<FuncionarioResponse>>> Listar(int? id, string? nome, string? telefone);
        Task<ResponseModel<FuncionarioResponse>> Inativar(int id);
        Task<ResponseModel<FuncionarioModel>> Login(LoginDto loginDto);
        Task<ResponseModel<string>> AlterarSenha(AlterarSenhaDto dto);

    }
}
