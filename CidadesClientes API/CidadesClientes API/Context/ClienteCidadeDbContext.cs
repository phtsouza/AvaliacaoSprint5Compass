using CidadesClientes_API.Models;
using CidadesClientes_API.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CidadesClientes_API.Context
{
    public class ClienteCidadeDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cidade> Cidades { get; set; }

        public ClienteCidadeDbContext(DbContextOptions<ClienteCidadeDbContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //modelBuilder.ApplyConfiguration(new CidadeConfiguration());
           //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        }
    }
}

