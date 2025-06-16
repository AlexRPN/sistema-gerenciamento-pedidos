using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.Cliente.Request;
using sistema_gerenciamento_pedidos.Dto.Cliente.Response;
using sistema_gerenciamento_pedidos.Dto.Empresa.Response;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Request;
using sistema_gerenciamento_pedidos.Dto.TelefoneCliente.Response;
using sistema_gerenciamento_pedidos.Models.Clientes;
using sistema_gerenciamento_pedidos.Models.Empresas;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;
using sistema_gerenciamento_pedidos.Models.TelefoneClientes;

namespace sistema_gerenciamento_pedidos.Profiles.Cliente
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteCriacaoDto, ClienteModel>().ReverseMap();

            CreateMap<ClienteModel, ClienteResponse>()
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.EnderecoCliente))
                .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src =>
                    src.TelefoneClientes != null
                        ? new TelefoneClienteResponse { Telefone = src.TelefoneClientes.Telefone }
                        : null))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()))
                .ForMember(dest => dest.PerfilAcesso, opt => opt.MapFrom(src => src.PerfilAcesso.ToString()));

            CreateMap<ClienteEdicaoDto, ClienteModel>()
                .ForMember(dest => dest.EnderecoCliente, opt => opt.MapFrom(src => src.EnderecoCliente));

            CreateMap<ClienteModel, ClienteEdicaoDto>()
                .ForMember(dest => dest.EnderecoCliente, opt => opt.MapFrom(src => src.EnderecoCliente));

            CreateMap<TelefoneClienteEdicaoDto, TelefoneClienteDto>();
            CreateMap<TelefoneClienteDto, TelefoneClienteResponse>();
            CreateMap<EnderecoClienteModel, EnderecoClienteResponse>();
            CreateMap<EmpresaModel, EmpresaResponse>();
        }
    }
}
