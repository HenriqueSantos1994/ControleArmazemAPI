using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ControleArmazem.Model;

namespace ControleArmazem.Infra
{
    public class CategoriaRepository
    {
        private readonly DynamoDBContext _context;

        public CategoriaRepository()
        {
            var client = new AmazonDynamoDBClient(); // usa credenciais e região padrão (do perfil AWS)
            _context = new DynamoDBContext(client);
        }

        // Criar ou atualizar uma categoria
        public async Task SalvarAsync(Categoria categoria)
        {
            if (categoria.Id == Guid.Empty)
                categoria.Id = Guid.NewGuid();

            await _context.SaveAsync(categoria);
        }

        // Obter categoria por Id
        public async Task<Categoria?> ObterPorIdAsync(Guid id)
        {
            return await _context.LoadAsync<Categoria>(id);
        }

        // Listar todas as categorias (scan — atenção ao custo em tabelas grandes)
        public async Task<List<Categoria>> ListarTodasAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Categoria>(conditions).GetRemainingAsync();
        }

        // Deletar categoria
        public async Task DeletarAsync(Guid id)
        {
            await _context.DeleteAsync<Categoria>(id);
        }
    }
}
