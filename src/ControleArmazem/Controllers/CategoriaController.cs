using ControleArmazem.Infra;
using ControleArmazem.Model;
using Microsoft.AspNetCore.Mvc;

namespace ControleArmazem.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly CategoriaRepository _categoriaRepository;
    private readonly ProdutoRepository _produtoRepository;

    public CategoriaController()
    {
        _categoriaRepository = new CategoriaRepository();
        _produtoRepository = new ProdutoRepository();
    }

    [HttpGet]
    public async Task<IEnumerable<Categoria>> Listar()
    {
        return await _categoriaRepository.ListarTodasAsync();
    }

    [HttpGet("{id}")]
    public async Task<Categoria> ObterPorId(Guid id)
    {
        return await _categoriaRepository.ObterPorIdAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task DeletarPorId(Guid id)
    {
        var produtos = await _produtoRepository.ListarPorCategoriaAsync(id);

        if(produtos.Any())
            throw new InvalidOperationException("Não é possível deletar uma categoria que possui produtos associados.");
        
        await _categoriaRepository.DeletarAsync(id);
    }

    [HttpPost]
    public async Task Salvar(Categoria categoria)
    {
        await _categoriaRepository.SalvarAsync(categoria);
    }

    [HttpPut]
    public async Task Atualizar(Categoria categoria)
    {
        await _categoriaRepository.DeletarAsync(categoria.Id);
        await _categoriaRepository.SalvarAsync(categoria);
    }
}
