using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Dto.Model.Response;

namespace sistema_gerenciamento_pedidos.Services.Funcionarios.Interfaces
{
    public interface IFuncionarioService
    {
        Task<ResponseModel<FuncionarioResponse>> Inserir(FuncionarioCriacaoDto funcionarioCriacaoDto);
    }
}
