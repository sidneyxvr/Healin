using Healin.Domain.Enums;
using Healin.Domain.ValueObjects;
using Healin.Shared.Data;
using Healin.Shared.Utils;
using System;

namespace Healin.Domain.Models
{
    public class Prescription : AuditableEntity
    {
        public string Description { get; private set; }
        public DateTime PrescriptionDate { get; private set; }
        public FilePath FilePath { get; private set; }
        public PrescriptionType PrescriptionType { get; private set; }
        public Guid SpecialtyId { get; private set; }
        public Guid PatientId { get; private set; }

        //EF Relational
        public Patient Patient { get; private set; }
        public Specialty Specialty { get; private set; }

        public static Prescription New(string description, DateTime prescriptionDate, Guid specialtyId, PrescriptionType prescriptionType, Guid patientId)
        {
            AssertionConcern.AssertArgumentNotEmpty(description, "A descrição é obrigatória");
            AssertionConcern.AssertArgumentLength(description, 100, "A descrição deve ter no máximo 100 caracteres");

            AssertionConcern.AssertArgumentRange(prescriptionDate, DateTime.Now.AddYears(-100), DateTime.Now, "Data da Prescrição inválida");

            AssertionConcern.AssertArgumentNotEmpty(specialtyId, "A especialidade é obrigatória");

            AssertionConcern.AssertIsEnum(prescriptionType, typeof(PrescriptionType), "Tipo de prescrição inválida");

            return new Prescription
            {
                Description = description,
                PrescriptionDate = prescriptionDate,
                SpecialtyId = specialtyId,
                PrescriptionType = prescriptionType,
                PatientId = patientId
            };
        }

        public void Update(string description, DateTime prescriptionDate, Guid specialtyId, PrescriptionType prescriptionType)
        {
            Description = description;
            PrescriptionDate = prescriptionDate;
            SpecialtyId = specialtyId;
            PrescriptionType = prescriptionType;
        }

        public void AddFilePath(string filePath) => FilePath = FilePath.CreatePDF(filePath);
    }
}
