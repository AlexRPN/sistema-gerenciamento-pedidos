using sistema_gerenciamento_pedidos.Models.Funcionarios;

namespace sistema_gerenciamento_pedidos.Services.Senha.Interface
{
    public interface ISenhaService
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
        string CriarToken(FuncionarioModel funcionarioModel);
    }
}
