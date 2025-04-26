using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Request;
using sistema_gerenciamento_pedidos.Dto.Funcionario.Response;
using sistema_gerenciamento_pedidos.Models.Funcionarios;

namespace sistema_gerenciamento_pedidos.Profiles.Funcionarios
{
    public class FuncionarioProfile : Profile
    {
        public FuncionarioProfile()
        {
            CreateMap<FuncionarioCriacaoDto, FuncionarioResponse>().ReverseMap();
            CreateMap<FuncionarioCriacaoDto, FuncionarioModel>().ReverseMap();
            CreateMap<FuncionarioModel, FuncionarioResponse>()
            .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.EnderecoFuncionario))
            .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa));
        }
    }
}
