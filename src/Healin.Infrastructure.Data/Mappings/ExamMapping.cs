using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class ExamMapping : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exam");

            builder.Property(exam => exam.Name).HasColumnType("varchar(50)");
        }
    }
}
