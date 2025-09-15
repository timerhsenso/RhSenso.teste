using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RhS.RHU.Core.Entities;

namespace RhS.Infrastructure.Configurations.RHU;

/// <summary>
/// Configuração do Entity Framework para a entidade Funcionario
/// </summary>
public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        // Configuração da tabela
        builder.ToTable("Funcionarios");

        // Chave primária
        builder.HasKey(f => f.Id);

        // Propriedades
        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.CPF)
            .IsRequired()
            .HasMaxLength(14); // 000.000.000-00

        builder.Property(f => f.Salario)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(f => f.DataAdmissao)
            .IsRequired();

        builder.Property(f => f.Cargo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Departamento)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Ativo");

        // Índices
        builder.HasIndex(f => f.Email)
            .IsUnique()
            .HasDatabaseName("IX_Funcionarios_Email");

        builder.HasIndex(f => f.CPF)
            .IsUnique()
            .HasDatabaseName("IX_Funcionarios_CPF");

        builder.HasIndex(f => f.Status)
            .HasDatabaseName("IX_Funcionarios_Status");

        builder.HasIndex(f => f.Departamento)
            .HasDatabaseName("IX_Funcionarios_Departamento");

        builder.HasIndex(f => f.DataAdmissao)
            .HasDatabaseName("IX_Funcionarios_DataAdmissao");
    }
}

