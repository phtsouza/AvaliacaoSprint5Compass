using CidadesClientes_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CidadesClientes_API.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cidades");

            builder
                .Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder
                .Property(c => c.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(max)")
                .IsRequired();

            builder
                .Property(c => c.Nascimento)
                .HasColumnName("Nascimento")
                .HasColumnType("datetime");

            builder
                .Property<Guid>("cidadeId")
                .IsRequired();

            builder
                .HasOne(cl => cl.cidade)
                .WithMany(ci => ci.clientes)
                .HasForeignKey("cidadeId");

            builder
                .Property(c => c.Cep)
                .HasColumnName("Cep")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder
                .Property(c => c.Bairro)
                .HasColumnName("Bairro")
                .HasColumnType("nvarchar(max)");

            builder
                .Property(c => c.Logradouro)
                .HasColumnName("Logradouro")
                .HasColumnType("nvarchar(max)");
        }
    }
}
