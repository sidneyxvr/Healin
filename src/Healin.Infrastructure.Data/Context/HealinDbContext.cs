using Healin.Domain.Models;
using Healin.Shared.Data;
using Healin.Shared.Intefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Context
{
    public class HealinDbContext : DbContext, IUnitOfWork
    {
        private readonly IAppUser _user;

        public HealinDbContext(DbContextOptions<HealinDbContext> options) : base(options)
        {
        }

        public HealinDbContext(DbContextOptions<HealinDbContext> options, IAppUser user) : base(options)
        {
            _user = user;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.Name == nameof(AuditableEntity.Created) || p.Name == nameof(AuditableEntity.LastModified))))
            {
                property.SetColumnType("smalldatetime");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealinDbContext).Assembly);

            modelBuilder.Entity<Doctor>()
                .HasMany(doctor => doctor.Specialties)
                .WithMany(specialty => specialty.Doctors)
                .UsingEntity<DoctorSpecialty>(
                    pt => pt.HasOne(a => a.Specialty).WithMany().HasForeignKey(d => d.SpecialtyId),
                    pt => pt.HasOne(a => a.Doctor).WithMany().HasForeignKey(d => d.DoctorId));

            modelBuilder.Entity<ExamResult>()
                .HasMany(examResult => examResult.ExamTypes)
                .WithMany(examType => examType.ExamResults)
                .UsingEntity<ExamResultExamType>(
                    pt => pt.HasOne(a => a.ExamType).WithMany().HasForeignKey(d => d.ExamTypeId),
                    pt => pt.HasOne(a => a.ExamResult).WithMany().HasForeignKey(d => d.ExamResultId));

            modelBuilder.Entity<Patient>()
                .HasMany(patient => patient.Doctors)
                .WithMany(doctor => doctor.Patients)
                .UsingEntity<DoctorPatient>(
                    pt => pt.HasOne(a => a.Doctor).WithMany().HasForeignKey(d => d.DoctorId),
                    pt => pt.HasOne(a => a.Patient).WithMany().HasForeignKey(d => d.PatientId));
        }

        public class DoctorSpecialty
        {
            public Guid DoctorId { get; set; }
            public Doctor Doctor { get; set; }
            public Guid SpecialtyId { get; set; }
            public Specialty Specialty { get; set; }
        }

        public class ExamResultExamType
        {
            public Guid ExamResultId { get; set; }
            public ExamResult ExamResult { get; set; }
            public Guid ExamTypeId { get; set; }
            public ExamType ExamType { get; set; }
        }

        public class DoctorPatient
        {
            public Guid DoctorId { get; set; }
            public Doctor Doctor { get; set; }
            public Guid PatientId { get; set; }
            public Patient Patient { get; set; }
        }

        public async Task<bool> CommitAsync()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (_user.IsAuthenticated)
                    {
                        entry.Property("CreatedById").CurrentValue = _user.UserId;
                    }
                    entry.Property("Created").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (_user.IsAuthenticated)
                    {
                        entry.Property("LastModifiedById").CurrentValue = _user.UserId;
                    }
                    entry.Property("LastModified").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync() > 0;
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<VaccineDose> VaccineCards { get; set; }
    }
}