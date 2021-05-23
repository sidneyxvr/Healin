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
    public class DoctorAppService : BaseAppService, IDoctorService
    {
        private readonly IAppUser _appUser;
        private readonly IFileStorage _fileStorage;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorReadOnlyRepository _doctorReadOnlyRepository;

        public DoctorAppService(
            IAppUser appUser,
            IFileStorage fileStorage,
            IDoctorRepository doctorRepository,
            IDoctorReadOnlyRepository doctorReadOnlyRepository,
            INotifier notifier,
            IMapper mapper) : base(notifier, mapper)
        {
            _appUser = appUser;
            _fileStorage = fileStorage;
            _doctorRepository = doctorRepository;
            _doctorReadOnlyRepository = doctorReadOnlyRepository;
        }

        public async Task AddAsync(DoctorRequest doctorRequest)
        {
            if (!ExecuteValidation(new DoctorRequestValidation(), doctorRequest))
            {
                return;
            }

            if (await _doctorRepository.EmailInUseAsync(doctorRequest.Email))
            {
                Notify("Email já está sendo utilizado por outro usuário");
                return;
            }

            if (await _doctorRepository.CpfInUseAsync(doctorRequest.Cpf))
            {
                Notify("CPF já está sendo utilizado por outro usuário");
                return;
            }

            if (await _doctorRepository.CrmInUseAsync(doctorRequest.Crm))
            {
                Notify("CRM já está sendo utilizado por outro médico");
                return;
            }

            var doctor = Doctor.New(doctorRequest.Id, doctorRequest.Name, doctorRequest.Cpf, doctorRequest.Email, 
                doctorRequest.Gender, doctorRequest.Phone, doctorRequest.BirthDate, doctorRequest.Crm);
            await _doctorRepository.AddAsync(doctor);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }

        public async Task DisableAsync(Guid id)
        {
            var patient = await _doctorRepository.GetByIdAsync(id);

            if (patient is null)
            {
                Notify("Paciente não encontrado");
                return;
            }

            patient.Disable();
            await _doctorRepository.UpdateAsync(patient);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }

        public async Task<AddressResponse> GetAddressAsync()
        {
            return Mapper.Map<AddressResponse>(await _doctorReadOnlyRepository.GetAddressAsync(_appUser.UserId));
        }

        public async Task<DoctorResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<DoctorResponse>(await _doctorRepository.GetByIdAsync(id));
        }

        public async Task<PagedListDTO<DoctorResponse>> GetPagedAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var doctors = await _doctorReadOnlyRepository.GetPagedAsync(page, pageSize, search, filter, order);

            return new PagedListDTO<DoctorResponse>
            (
                Mapper.Map<ICollection<DoctorResponse>>(doctors.Collection),
                doctors.Amount
            );
        }

        public async Task<PagedListDTO<DoctorResponse>> GetPagedByPatientAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var doctors = await _doctorReadOnlyRepository.GetPagedByPatientIdAsync(_appUser.UserId, page, pageSize, search, filter, order);

            return new PagedListDTO<DoctorResponse>
            (
                Mapper.Map<ICollection<DoctorResponse>>(doctors.Collection),
                doctors.Amount
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor is null)
            {
                Notify("Médico não encontrado");
                return;
            }

            doctor.Delete();

            await _doctorRepository.UpdateAsync(doctor);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAddressAsync(AddressRequest addressRequest)
        {
            if (!ExecuteValidation(new AddressRequestValidation(), addressRequest))
            {
                return;
            }

            var doctorId = _appUser.UserId;

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            doctor.UpdateAddress(addressRequest.Street, addressRequest.Number, addressRequest.District,
                addressRequest.City, addressRequest.State, addressRequest.PostalCode, addressRequest.Complement);

            await _doctorRepository.UpdateAsync(doctor);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(DoctorRequest doctorRequest)
        {
            if (!ExecuteValidation(new DoctorRequestValidation(), doctorRequest))
            {
                return;
            }

            var doctor = await _doctorRepository.GetByIdAsync(doctorRequest.Id);

            if (doctor is null)
            {
                Notify("Médico não encontrado");
                return;
            }

            if (await _doctorRepository.EmailInUseAsync(doctor.Id, doctorRequest.Email))
            {
                Notify("Email já está sendo utilizado por outro usuário");
                return;
            }

            if (await _doctorRepository.CpfInUseAsync(doctor.Id, doctorRequest.Cpf))
            {
                Notify("CPF já está sendo utilizado por outro usuário");
                return;
            }

            if (await _doctorRepository.CrmInUseAsync(doctor.Id, doctorRequest.Crm))
            {
                Notify("CRM já está sendo utilizado por outro médico");
                return;
            }

            doctor.Update(doctorRequest.Name, doctorRequest.Cpf, doctorRequest.Gender,
                doctorRequest.Phone, doctor.BirthDate, doctorRequest.Crm);

            if (doctorRequest.ImageUpload is not null && doctorRequest.ImageUpload.Length > 0)
            {
                var imageResult = await _fileStorage.SaveImage(doctorRequest.ImageUpload);

                if (!imageResult.Succeeded)
                {
                    imageResult.Errors.ForAll(Notify);
                    return;
                }
                doctor.AddImagePath(imageResult.Result);
            }

            await _doctorRepository.UpdateAsync(doctor);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateImageAsync(IFormFile image)
        {
            if (image is null || image.Length == 0)
            {
                Notify("Imagem obrigatória");
                return;
            }

            var doctor = await _doctorRepository.GetByIdAsync(_appUser.UserId);

            if (doctor is null)
            {
                Notify("Médico não encontrado");
                return;
            }

            var result = await _fileStorage.SaveImage(image);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(Notify);
                return;
            }

            doctor.AddImagePath(result.Result);

            await _doctorRepository.UpdateAsync(doctor);
            await _doctorRepository.UnitOfWork.CommitAsync();
        }
    }
}
