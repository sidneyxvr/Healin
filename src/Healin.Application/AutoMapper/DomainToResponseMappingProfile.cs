using AutoMapper;
using Healin.Application.Responses;
using Healin.Domain.Enums;
using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using System;
using System.Linq;

namespace Healin.Application.AutoMapper
{
    public class DomainToResponseMappingProfile : Profile
    {
        public DomainToResponseMappingProfile()
        {
            CreateMap<Address, AddressResponse>();

            CreateMap<Doctor, DoctorResponse>()
                .ForMember(doctorResponse => doctorResponse.Marked, opt => opt.MapFrom(doctor => doctor.Patients.Any()))
                .ForMember(doctorResponse => doctorResponse.GenderDescription, opt => opt.MapFrom(doctor => doctor.Gender.GetDescription()));

            CreateMap<Exam, ExamResponse>();
            CreateMap<Exam, SelectItem<Guid>>()
                .ForMember(selectItem => selectItem.Value, opt => opt.MapFrom(exam => exam.Id))
                .ForMember(selectItem => selectItem.Text, opt => opt.MapFrom(exam => exam.Name));

            CreateMap<ExamResult, ExamResultResponse>()
                .ForMember(examResultResponse => examResultResponse.Exam, opt => opt.MapFrom(examResult => examResult.Exam.Name));

            CreateMap<ExamType, ExamTypeResponse>();
            CreateMap<ExamType, SelectItem<Guid>>()
                .ForMember(selectItem => selectItem.Value, opt => opt.MapFrom(examType => examType.Id))
                .ForMember(selectItem => selectItem.Text, opt => opt.MapFrom(examType => examType.Name));

            CreateMap<Patient, PatientResponse>()
                .ForMember(patientResponse => patientResponse.GenderDescription, opt => opt.MapFrom(patient => patient.Gender.GetDescription()));

            CreateMap<Prescription, PrescriptionResponse>()
                .ForMember(prescription => prescription.PrescriptionType, opt => opt.MapFrom(prescriptionResponse => prescriptionResponse.PrescriptionType))
                .ForMember(prescription => prescription.PrescriptionTypeDescription, opt => opt.MapFrom(prescriptionResponse => prescriptionResponse.PrescriptionType.GetDescription()));

            CreateMap<Specialty, SpecialtyResponse>();
            CreateMap<Specialty, SelectItem<Guid>>()
                .ForMember(selectItem => selectItem.Value, opt => opt.MapFrom(examType => examType.Id))
                .ForMember(selectItem => selectItem.Text, opt => opt.MapFrom(examType => examType.Name));

            CreateMap<Vaccine, VaccineResponse>();
            CreateMap<Vaccine, SelectItem<Guid>>()
                .ForMember(selectItem => selectItem.Value, opt => opt.MapFrom(examType => examType.Id))
                .ForMember(selectItem => selectItem.Text, opt => opt.MapFrom(examType => examType.Name));

            CreateMap<VaccineDose, VaccineDoseResponse>()
                .ForMember(vaccineDoseResponse => vaccineDoseResponse.DoseType, opt => opt.MapFrom(vaccineDose => vaccineDose.DoseType))
                .ForMember(vaccineDoseResponse => vaccineDoseResponse.DoseTypeDescription, opt => opt.MapFrom(vaccineDose => vaccineDose.DoseType.GetDescription()));
        }
    }
}
