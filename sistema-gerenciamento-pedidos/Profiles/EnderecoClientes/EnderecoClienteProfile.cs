using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.EnderecoCliente.Response;
using sistema_gerenciamento_pedidos.Models.EnderecoCliente;

namespace sistema_gerenciamento_pedidos.Profiles.EnderecoClientes
{
    public class EnderecoClienteProfile : Profile
    {
        public EnderecoClienteProfile()
        {
            CreateMap<EnderecoClienteModel, EnderecoClienteResponse>().ReverseMap();
        }
    }
}
