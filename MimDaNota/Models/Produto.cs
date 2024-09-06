namespace MimDaNota.Models
{
    public class Produto
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoDescricao { get; set; }
        public string ProdutoNome { get; set; }
        public decimal ProdutoPreco { get; set; }
        public int ProdutoEstoque { get; set; }

        /*************************************************/

        public Guid FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        /*************************************************/

        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
