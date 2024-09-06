using System.ComponentModel.DataAnnotations.Schema;

namespace MimDaNota.Models
{
    public class NotaFiscal
    {

        public Guid Id { get; set; }
        public int Quantidade { get; set; }

        /*************************************************/

        public Guid ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public decimal ValorTotal => Quantidade * Produto?.ProdutoPreco ?? 0;

        /*************************************************/

        public Guid UsuarioId { get; set; }
        public User? Usuario { get; set; }

        /*************************************************/
    }
}
