using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ControleArmazem.Model;

namespace ControleArmazem.Infra
{
    public class ProdutoRepository
    {
        private readonly DynamoDBContext _context;

        public ProdutoRepository()
        {
            var client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(client);
        }

        /// <summary>
        /// Cria ou atualiza um produto.
        /// </summary>
        public async Task SalvarAsync(Produto produto)
        {
            if (produto.Id == Guid.Empty)
                produto.Id = Guid.NewGuid();

            await _context.SaveAsync(produto);
        }

        /// <summary>
        /// Busca um produto pelo ID.
        /// </summary>
        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _context.LoadAsync<Produto>(id);
        }

        /// <summary>
        /// Lista todos os produtos (cuidado com tabelas grandes).
        /// </summary>
        public async Task<List<Produto>> ListarTodosAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Produto>(conditions).GetRemainingAsync();
        }

        /// <summary>
        /// Deleta um produto pelo ID.
        /// </summary>
        public async Task DeletarAsync(Guid id)
        {
            await _context.DeleteAsync<Produto>(id);
        }

        /// <summary>
        /// Lista todos os produtos de uma categoria específica.
        /// </summary>
        public async Task<List<Produto>> ListarPorCategoriaAsync(Guid categoriaId)
        {
            var conditions = new List<ScanCondition>
            {
                new(nameof(Produto.CategoriaId), Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, categoriaId)
            };

            return await _context.ScanAsync<Produto>(conditions).GetRemainingAsync();
        }
    }
}