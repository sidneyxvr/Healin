using Healin.Domain.Enums;
using System;

namespace Healin.Application.Responses
{
    public class VaccineDoseResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; private set; }
        public Guid VaccineId { get; private set; }
        public VaccineResponse Vaccine { get; private set; }
        public DoseType DoseType { get; private set; }
        public string DoseTypeDescription { get; private set; }
        public DateTime DoseDate { get; private set; }
    }
}
