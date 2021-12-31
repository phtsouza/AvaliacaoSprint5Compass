using CidadesClientes_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CidadesClientes_API.Context
{
    public class ClienteCidadeDbContext : DbContext
    {
        public ClienteCidadeDbContext(DbContextOptions<ClienteCidadeDbContext> options) : base(options){}

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
    }
}

