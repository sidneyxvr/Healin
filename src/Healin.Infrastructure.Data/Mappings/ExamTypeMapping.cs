using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class ExamTypeMapping : IEntityTypeConfiguration<ExamType>
    {
        public void Configure(EntityTypeBuilder<ExamType> builder)
        {
            builder.ToTable("ExamType");

            builder.Property(examType => examType.Name).HasColumnType("varchar(50)");

            builder.HasOne(examType => examType.Exam)
                   .WithMany(exam => exam.ExamTypes)
                   .HasForeignKey(examType => examType.ExamId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
