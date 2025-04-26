using AutoMapper;
using sistema_gerenciamento_pedidos.Dto.Produto.Request;
using sistema_gerenciamento_pedidos.Dto.Produto.Response;
using sistema_gerenciamento_pedidos.Models.Produtos;

namespace sistema_gerenciamento_pedidos.Profiles.Produtos
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoCriacaoDto, ProdutoModel>().ReverseMap();
            CreateMap<ProdutoModel, ProdutoResponse>().ReverseMap()
                .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa));
            CreateMap<ProdutoEdicaoDto, ProdutoModel>().ReverseMap();
        }
    }
}
