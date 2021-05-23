using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class SpecialtyMapping : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable("Specialty");

            builder.Property(specialty => specialty.Name).HasColumnType("varchar(50)");
        }
    }
}
