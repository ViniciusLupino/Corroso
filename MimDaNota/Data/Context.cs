using Microsoft.EntityFrameworkCore;
using MimDaNota.Models;

namespace MimDaNota.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<NotaFiscal> NotaFiscal { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tbUser");
            modelBuilder.Entity<Fornecedor>().ToTable("tbFornecedor");
            modelBuilder.Entity<Categoria>().ToTable("tbCategoria");
            modelBuilder.Entity<Produto>().ToTable("tbProduto");
            modelBuilder.Entity<NotaFiscal>().ToTable("tbNotaFiscal");
        }
    }
}

