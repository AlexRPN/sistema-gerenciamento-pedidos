﻿using sistema_gerenciamento_pedidos.Dto.Model.Response;
using sistema_gerenciamento_pedidos.Dto.Produto.Request;
using sistema_gerenciamento_pedidos.Dto.Produto.Response;
using sistema_gerenciamento_pedidos.Enums;
using sistema_gerenciamento_pedidos.Models.Produtos;

namespace sistema_gerenciamento_pedidos.Services.Produtos.Interfaces
{
    public interface IProdutoService
    {
        Task<ResponseModel<ProdutoResponse>> Cadastrar(ProdutoCriacaoDto produtoCriacaoDto);
        Task<ResponseModel<List<ProdutoResponse>>> Listar(CategoriaEnum? categoria);
        Task<ResponseModel<ProdutoResponse>> Editar(ProdutoEdicaoDto produtoEdicaoDto, IFormFile? imagem);
        Task<ResponseModel<ProdutoResponse>> BuscarProdutoPorId(int id);
        Task<ResponseModel<ProdutoModel>> Inativar(int id);
    }
}
