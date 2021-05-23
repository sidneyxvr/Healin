using Healin.Domain.Enums;
using Healin.Shared.Data;
using Healin.Shared.Utils;
using System;

namespace Healin.Domain.Models
{
    public class VaccineDose : AuditableEntity
    {
        public Guid PatientId { get; private set; }
        public Patient Patient { get; private set; }
        public Guid VaccineId { get; private set; }
        public Vaccine Vaccine { get; private set; }
        public DoseType DoseType { get; private set; }
        public DateTime DoseDate { get; private set; }

        public static VaccineDose New(Guid patientId, Guid vaccineId, DoseType doseType, DateTime doseDate)
        {
            AssertionConcern.AssertArgumentNotEmpty(patientId, "O paciente é obrigatório");

            AssertionConcern.AssertArgumentNotEmpty(vaccineId, "A vacina é obrigatória");

            AssertionConcern.AssertIsEnum(doseType, typeof(DoseType), "Tipo da Dose inválida");

            AssertionConcern.AssertArgumentRange(doseDate, DateTime.Now.AddYears(-100), DateTime.Now, "Data da Dose inválida");

            return new VaccineDose
            {
                PatientId = patientId,
                VaccineId = vaccineId,
                DoseType = doseType,
                DoseDate = doseDate.Date,
            };
        }
    }
}
