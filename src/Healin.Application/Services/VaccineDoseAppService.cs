using AutoMapper;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Validations;
using Healin.Application.Responses;
using Healin.Domain.Enums;
using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Application.Services
{
    public class VaccineDoseAppService : BaseAppService, IVaccineDoseService
    {
        private readonly IAppUser _appUser;
        private readonly IPatientRepository _patientRepository;
        private readonly IVaccineDoseRepository _vaccineCardRepository;
        private readonly IVaccineDoseReadOnlyRepository _vaccineCardReadOnlyRepository;
        public VaccineDoseAppService(
            IPatientRepository patientRepository,
            IVaccineDoseRepository vaccineCardRepository, 
            IVaccineDoseReadOnlyRepository vaccineCardReadOnlyRepository, 
            INotifier notifier, IAppUser appUser,
            IMapper mapper) : base(notifier, mapper)
        {
            _appUser = appUser;
            _patientRepository = patientRepository;
            _vaccineCardRepository = vaccineCardRepository;
            _vaccineCardReadOnlyRepository = vaccineCardReadOnlyRepository;
        }

        public async Task AddAsync(VaccineDoseRequest vaccineDoseRequest)
        {
            if (!ExecuteValidation(new VaccineDoseRequestValidation(), vaccineDoseRequest))
            {
                return;
            }

            var doses = await _vaccineCardRepository.GetByPatientIdAndVaccineIdAsync(_appUser.UserId, vaccineDoseRequest.VaccineId);

            if (doses.Any(dose => dose.DoseType == vaccineDoseRequest.DoseType))
            {
                Notify("Dose já cadastrada");
                return;
            }

            var dose = VaccineDose.New(_appUser.UserId, vaccineDoseRequest.VaccineId, vaccineDoseRequest.DoseType, vaccineDoseRequest.DoseDate);

            await _vaccineCardRepository.AddAsync(dose);
            await _vaccineCardRepository.UnitOfWork.CommitAsync();
        }

        public Task<IEnumerable<SelectItem<string>>> GetDoseTypesAsync()
        {
            return Task.FromResult(
                Enum.GetValues(typeof(DoseType))
                .Cast<DoseType>()
                .Select(p => new SelectItem<string>
                {
                    Text = p.GetDescription(),
                    Value = ((int)p).ToString()
                }));
        }

        public async Task<IEnumerable<(string VaccineName, List<VaccineDoseResponse> VaccineDoses)>> GetByLoggedPatientAsync()
        {
            var patientId = _appUser.UserId;

            return Mapper.Map<IEnumerable<VaccineDoseResponse>>(await _vaccineCardReadOnlyRepository.GetByPatientIdAsync(patientId))
                .GroupBy(vaccineDose => vaccineDose.Vaccine.Name)
                .Select(a => (VaccineName: a.Key, VaccineDoses: a.OrderBy(vaccine => vaccine.DoseType).ToList()))
                .OrderBy(vaccineDose => vaccineDose.VaccineName);
        }

        public async Task<IEnumerable<(string VaccineName, List<VaccineDoseResponse> VaccineDoses)>> GetByPatientIdAsync(Guid patientId)
        {
            var doctorId = _appUser.UserId;

            var patient = await _patientRepository.GetByIdAsync(patientId, nameof(Patient.Doctors));

            if (patient.Doctors.All(d => d.Id != doctorId))
            {
                return new List<(string VaccineName, List<VaccineDoseResponse> VaccineDoses)>();
            }

            return Mapper.Map<IEnumerable<VaccineDoseResponse>>(await _vaccineCardReadOnlyRepository.GetByPatientIdAsync(patientId))
                .GroupBy(vaccineDose => vaccineDose.Vaccine.Name)
                .Select(a => (VaccineName: a.Key, VaccineDoses: a.OrderBy(vaccine => vaccine.DoseType).ToList()))
                .OrderBy(vaccineDose => vaccineDose.VaccineName);
        }

        public async Task RemoveAsync(Guid id)
        {
            var vaccineDose = await _vaccineCardRepository.GetByIdAsync(id);

            if(vaccineDose is null)
            {
                Notify("Dose não encontrada");
                return;
            }

            await _vaccineCardRepository.RemoveAsync(vaccineDose);
            await _vaccineCardRepository.UnitOfWork.CommitAsync();
        }
    }
}
