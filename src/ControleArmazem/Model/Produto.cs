using Amazon.DynamoDBv2.DataModel;

namespace ControleArmazem.Model
{
    [DynamoDBTable("Produtos")]
    public class Produto
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty]
        public string Nome { get; set; }

        [DynamoDBProperty]
        public decimal Quatidade { get; set; }

        [DynamoDBProperty]
        public Guid CategoriaId { get; set; }

        [DynamoDBIgnore]
        public Categoria Categoria { get; set; }
    }
}
