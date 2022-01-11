using CidadesClientesServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadesClientesServices.Configurations
{
    public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
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
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder
                .Property(c => c.Estado)
                .HasColumnName("Estado")
                .HasColumnType("nvarchar(max)")
                .IsRequired();
        }
    }
}