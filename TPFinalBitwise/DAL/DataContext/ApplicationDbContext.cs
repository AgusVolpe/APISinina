using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Venta>().Property(x => x.FechaRealizacion).HasColumnType("date");

            modelBuilder.Entity<Categoria>().Property(x => x.Nombre).HasMaxLength(350);
        }*/

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Producto> Productos { get; set; }

        //public DbSet<Venta> Ventas { get; set; }
        //public DbSet<Usuario> Usuarios { get; set; }
    }
}
