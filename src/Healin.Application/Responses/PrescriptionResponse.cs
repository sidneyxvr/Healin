using Healin.Domain.Enums;
using System;

namespace Healin.Application.Responses
{
    public class PrescriptionResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public Guid SpecialtyId { get; set; }
        public SelectItem<Guid> Specialty { get; set; }
        public string FilePath { get; set; }
        public PrescriptionType PrescriptionType { get; set; }
        public string PrescriptionTypeDescription { get; set; }
    }
}
