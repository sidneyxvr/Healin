using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class VaccineDoseMapping : IEntityTypeConfiguration<VaccineDose>
    {
        public void Configure(EntityTypeBuilder<VaccineDose> builder)
        {
            builder.ToTable("VaccineCard");

            builder.Property(examResult => examResult.DoseDate).HasColumnType("date");
        }
    }
}
