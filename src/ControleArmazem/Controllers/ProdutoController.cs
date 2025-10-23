using ControleArmazem.Infra;
using ControleArmazem.Model;
using Microsoft.AspNetCore.Mvc;

namespace ControleArmazem.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoRepository _produtoRepository;
    private readonly CategoriaRepository _categoriaRepository;

    public ProdutoController()
    {
        _produtoRepository = new ProdutoRepository();
        _categoriaRepository = new CategoriaRepository();
    }

    [HttpGet]
    public async Task<IEnumerable<Produto>> Listar()
    {
        var produtos = await _produtoRepository.ListarTodosAsync();

        produtos.ForEach(async p =>
        {
            p.Categoria = await _categoriaRepository.ObterPorIdAsync(p.CategoriaId);
        });

        return produtos;
    }

    [HttpGet("{idCategoria}")]
    public async Task<IEnumerable<Produto>> ListarPorCategoria(Guid idCategoria)
    {
        var produtos = await _produtoRepository.ListarPorCategoriaAsync(idCategoria);

        produtos.ForEach(async p =>
        {
            p.Categoria = await _categoriaRepository.ObterPorIdAsync(p.CategoriaId);
        });

        return produtos;
    }

    [HttpGet("{id}")]
    public async Task<Produto> ObterPorId(Guid id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);

        produto.Categoria = await _categoriaRepository.ObterPorIdAsync(produto.CategoriaId);

        return produto;
    }

    [HttpDelete("{id}")]
    public async Task DeletarPorId(Guid id)
    {
        await _produtoRepository.DeletarAsync(id);
    }

    [HttpPost]
    public async Task Salvar(Produto produto)
    {
        await _produtoRepository.SalvarAsync(produto);
    }

    [HttpPut]
    public async Task Atualizar(Produto produto)
    {
        await _produtoRepository.DeletarAsync(produto.Id);
        await _produtoRepository.SalvarAsync(produto);
    }
}
