using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class VaccineMapping : IEntityTypeConfiguration<Vaccine>
    {
        public void Configure(EntityTypeBuilder<Vaccine> builder)
        {
            builder.ToTable("Vaccine");

            builder.Property(vaccine => vaccine.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();
        }
    }
}
