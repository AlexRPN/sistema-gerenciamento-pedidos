using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Request;
using sistema_gerenciamento_pedidos.Dto.EnderecoFuncionario.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoFuncionario;

namespace sistema_gerenciamento_pedidos.Profiles.EnderecoFuncionario
{
    public class EnderecoFuncionarioProfile : Profile
    {
        public EnderecoFuncionarioProfile()
        {
            CreateMap<EnderecoFuncionarioModel, EnderecoFuncionarioResponse>();
            CreateMap<EnderecoFuncionarioEdicaoDto, EnderecoFuncionarioModel>().ReverseMap();
        }
    }
}
