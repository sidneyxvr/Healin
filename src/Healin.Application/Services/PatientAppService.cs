using AutoMapper;
using AutoMapper.Internal;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Validations;
using Healin.Application.Responses;
using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Storage;
using Healin.Shared.DTOs;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Application.Services
{
    public class PatientAppService : BaseAppService, IPatientService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IPatientReadOnlyRepository _patientReadOnlyRepository;
        private readonly IAppUser _appUser;

        public PatientAppService(
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IFileStorage fileStorage,
            IPatientReadOnlyRepository patientReadOnlyRepository,
            IAppUser appUser, INotifier notifier, IMapper mapper) : base(notifier, mapper)
        {
            _appUser = appUser;
            _fileStorage = fileStorage;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _patientReadOnlyRepository = patientReadOnlyRepository;
        }

        public async Task AddAsync(PatientRequest patientRequest)
        {
            if (!ExecuteValidation(new PatientRequestValidation(), patientRequest))
            {
                return;
            }

            if (await _patientRepository.EmailInUseAsync(patientRequest.Email))
            {
                Notify("Email já está sendo utilizado por outro usuário");
                return;
            }

            if (await _patientRepository.CpfInUseAsync(patientRequest.Cpf))
            {
                Notify("CPF já está sendo utilizado por outro usuário");
                return;
            }

            if (await _patientRepository.SusNumberInUseAsync(patientRequest.SusNumber))
            {
                Notify("Número do SUS já está sendo utilizado por outro usuário");
                return;
            }

            var patient = Patient.New(patientRequest.Id, patientRequest.Name, 
                patientRequest.Cpf, patientRequest.Email, patientRequest.Gender, 
                patientRequest.Phone, patientRequest.BirthDate, patientRequest.SusNumber);

            await _patientRepository.AddAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PagedListDTO<PatientResponse>> GetPagedByDoctorIdAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var doctor = _appUser.UserId;

            var patients = await _patientReadOnlyRepository.GetPagedByDoctorIdAsync(doctor, page, pageSize, search, filter, order);
            return new PagedListDTO<PatientResponse>
            (
                Mapper.Map<ICollection<PatientResponse>>(patients.Collection),
                patients.Amount
            );
        }

        public async Task<PatientResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<PatientResponse>(await _patientRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(PatientRequest patientRequest)
        {
            var patient = await _patientRepository.GetByIdAsync(patientRequest.Id);

            patient.Delete();

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PatientRequest patientRequest)
        {
            if (!ExecuteValidation(new PatientRequestValidation(), patientRequest))
            {
                return;
            }

            var patient = await _patientRepository.GetByIdAsync(patientRequest.Id);

            if(patient is null)
            {
                Notify("Paciente não encontrado");
                return;
            }

            if(await _patientRepository.EmailInUseAsync(patient.Id, patientRequest.Email))
            {
                Notify("Email já está sendo utilizado por outro usuário");
                return;
            }

            if (await _patientRepository.CpfInUseAsync(patient.Id, patientRequest.Cpf))
            {
                Notify("CPF já está sendo utilizado por outro usuário");
                return;
            }

            if (await _patientRepository.SusNumberInUseAsync(patient.Id, patientRequest.SusNumber))
            {
                Notify("Número do SUS já está sendo utilizado por outro usuário");
                return;
            }

            patient.Update(patientRequest.Name, patientRequest.Cpf, patientRequest.BirthDate, 
                patientRequest.Gender, patientRequest.Phone, patientRequest.SusNumber);

            if (patientRequest.ImageUpload is not null && patientRequest.ImageUpload.Length > 0)
            {
                var imageResult = await _fileStorage.SaveImage(patientRequest.ImageUpload);

                if (!imageResult.Succeeded)
                {
                    imageResult.Errors.ForAll(Notify);
                    return;
                }
                patient.AddImagePath(imageResult.Result);
            }

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAddressAsync(AddressRequest addressRequest)
        {
            if (!ExecuteValidation(new AddressRequestValidation(), addressRequest))
            {
                return;
            }

            var patientId = _appUser.UserId;

            var patient = await _patientRepository.GetByIdAsync(patientId);

            patient.UpdateAddress(addressRequest.Street, addressRequest.Number, addressRequest.District, 
                addressRequest.City, addressRequest.State, addressRequest.PostalCode, addressRequest.Complement);

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task AddDoctorToMyDoctorsAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                Notify("Médico não encontrado");
                return;
            }

            var patient = await _patientRepository.GetByIdAsync(_appUser.UserId, nameof(Patient.Doctors));
            patient.AddDoctor(doctor);

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task RemoveDoctorFromMyDoctorsAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                Notify("Médico não encontrado");
                return;
            }

            var patient = await _patientRepository.GetByIdAsync(_appUser.UserId, nameof(Patient.Doctors));
            patient.RemoveDoctor(doctor);

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task<AddressResponse> GetAddressAsync()
        {
            return Mapper.Map<AddressResponse>(await _patientReadOnlyRepository.GetAddressAsync(_appUser.UserId));
        }

        public async Task DisableAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if(patient is null)
            {
                Notify("Paciente não encontrado");
                return;
            }

            patient.Disable();
            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateImageAsync(IFormFile image)
        {
            if(image is null || image.Length == 0)
            {
                Notify("Não foi possível alterar a imagem");
                return;
            }

            var patient = await _patientRepository.GetByIdAsync(_appUser.UserId);

            if (patient is null)
            {
                Notify("Paciente não encontrado");
                return;
            }

            var result = await _fileStorage.SaveImage(image);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(Notify);
                return;
            }

            patient.AddImagePath(result.Result);

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.UnitOfWork.CommitAsync();
        }
    }
}
