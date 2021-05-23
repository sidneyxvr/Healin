using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class PrescriptionMapping : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.ToTable("Prescription");

            builder.Property(examResult => examResult.Description).HasColumnType("varchar(100)");

            builder.Property(examResult => examResult.PrescriptionDate).HasColumnType("date");

            builder.OwnsOne(user => user.FilePath, imagePath =>
            {
                imagePath.Property(e => e.Path).HasColumnType("varchar(200)").HasColumnName("FilePath");
            });
        }
    }
}
