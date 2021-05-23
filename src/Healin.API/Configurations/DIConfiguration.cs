using Healin.API.Extensions;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Services;
using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Infrastructure.Storage;
using Healin.Infrastructure.Data.Repositories;
using Healin.Infrastructure.Data.Repositories.ReadOnly;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Healin.API.Configurations
{
    public static class DIConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            if(configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddScoped<IAppUser, AppUser>();
            services.AddScoped<INotifier, Notifier>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IDoctorService, DoctorAppService>();
            services.AddScoped<IExamService, ExamAppService>();
            services.AddScoped<IExamResultService, ExamResultAppService>();
            services.AddScoped<IExamTypeService, ExamTypeAppService>();
            services.AddScoped<IPatientService, PatientAppService>();
            services.AddScoped<ISpecialtyService, SpecialtyAppService>();
            services.AddScoped<IPrescriptionService, PrescriptionAppService>();
            services.AddScoped<IVaccineService, VaccineAppService>();
            services.AddScoped<IVaccineDoseService, VaccineDoseAppService>();

            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IExamResultRepository, ExamResultRepository>();
            services.AddScoped<IExamTypeRepository, ExamTypeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IVaccineDoseRepository, VaccineDoseRepository>();

            services.AddScoped<IDoctorReadOnlyRepository, DoctorReadOnlyRepository>();
            services.AddScoped<IExamReadOnlyRepository, ExamReadOnlyRepository>();
            services.AddScoped<IExamResultReadOnlyRepository, ExamResultReadOnlyRepository>();
            services.AddScoped<IExamTypeReadOnlyRepository, ExamTypeReadOnlyRepository>();
            services.AddScoped<IPatientReadOnlyRepository, PatientReadOnlyRepository>();
            services.AddScoped<ISpecialtyReadOnlyRepository, SpecialtyReadOnlyRepository>();
            services.AddScoped<IPrescriptionReadOnlyRepository, PrescriptionReadOnlyRepository>();
            services.AddScoped<IVaccineReadOnlyRepository, VaccineReadOnlyRepository>();
            services.AddScoped<IVaccineDoseReadOnlyRepository, VaccineDoseReadOnlyRepository>();

            services.AddScoped<IFileStorage, FileStorage>();
        }
    }
}
