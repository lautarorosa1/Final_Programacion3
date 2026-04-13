using Microsoft.EntityFrameworkCore;
using Backend_TrabajoFinal.Models;

namespace Backend_TrabajoFinal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //CUANDO BORRO UN CLIENTE SE BORRAN TODAS SUS TRANSACCIONES
        {
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Cliente)
                .WithMany(c => c.Transacciones)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaccion>() //QUE TIPO TRANSACCION SEA STRING
                .Property(t => t.TipoTransaccion)
                .HasConversion<string>();
        }
    }
}