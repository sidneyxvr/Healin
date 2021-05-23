using AutoMapper;
using Healin.Application.Requests;
using Healin.Domain.Models;

namespace Healin.Application.AutoMapper
{
    public class RequestToDomainMappingProfile : Profile
    {
        public RequestToDomainMappingProfile()
        {
            CreateMap<ExamRequest, Exam>()
                .ConvertUsing(examRequest => Exam.New(examRequest.Name));

            CreateMap<ExamTypeRequest, ExamType>()
                .ConvertUsing(examTypeRequest => ExamType.New(examTypeRequest.Name, examTypeRequest.ExamId));

            CreateMap<SpecialtyRequest, Specialty>()
                .ConvertUsing(specialtyRequest => Specialty.New(specialtyRequest.Name));
            
            CreateMap<VaccineRequest, Vaccine>()
                .ConvertUsing(vaccineRequest => Vaccine.New(vaccineRequest.Name));
        }
    }
}
