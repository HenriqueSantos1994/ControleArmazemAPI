using Amazon.DynamoDBv2.DataModel;

namespace ControleArmazem.Model
{
    [DynamoDBTable("Categorias")]
    public class Categoria
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty]
        public string Nome { get; set; }
    }
}
