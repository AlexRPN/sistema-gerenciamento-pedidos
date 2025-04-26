using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.Empresa.Request;
using sistema_gerenciamento_pedidos.Dto.Empresa.Response;
using sistema_gerenciamento_pedidos.Models.Empresas;

namespace sistema_gerenciamento_pedidos.Profiles.Empresa
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<EmpresaCriacaoDto, EmpresaModel>().ReverseMap();
            CreateMap<EmpresaResponse, EmpresaModel>().ReverseMap();
        }
    }
}
