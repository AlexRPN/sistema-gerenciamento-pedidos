using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Models.Clientes;

namespace sistema_gerenciamento_pedidos.Profiles.Cliente
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteCriacaoDto, ClienteModel>().ReverseMap();
            CreateMap<ClienteModel, ClienteResponse>()
            .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.EnderecoCliente))
            .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa));
        }
    }
}
