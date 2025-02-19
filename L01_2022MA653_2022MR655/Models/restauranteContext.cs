using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace L01_2022MA653_2022MR655.Models
{
    public class restauranteContext : DbContext
    {
        public restauranteContext(DbContextOptions<restauranteContext> options) : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }

        public virtual DbSet<Motorista> Motoristas { get; set; }

        public virtual DbSet<Pedido> Pedidos { get; set; }

        public virtual DbSet<Plato> Platos { get; set; }
    }
}
