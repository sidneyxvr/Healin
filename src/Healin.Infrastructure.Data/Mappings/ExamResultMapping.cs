using Healin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class ExamResultMapping : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> builder)
        {
            builder.ToTable("ExamResult");

            builder.Property(examResult => examResult.Description).HasColumnType("varchar(100)");

            builder.OwnsOne(user => user.FilePath, imagePath =>
            {
                imagePath.Property(e => e.Path).HasColumnType("varchar(200)").HasColumnName("FilePath");
            });

            builder.Property(examResult => examResult.ExamDate).HasColumnType("date");

            builder.HasOne(examResult => examResult.Patient)
                   .WithMany(patient => patient.Exams)
                   .HasForeignKey(examResult => examResult.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(examResult => examResult.Exam)
                   .WithMany(exam => exam.ExamResults)
                   .HasForeignKey(examResult => examResult.ExamId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
