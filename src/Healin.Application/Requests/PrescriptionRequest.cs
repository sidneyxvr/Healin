using Healin.Domain.Enums;
using System;

namespace Healin.Application.Requests
{
    public class PrescriptionRequest : RequestBase
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public Guid SpecialtyId { get; set; }
        public string FilePath { get; set; }
        public PrescriptionType PrescriptionType { get; set; }
    }
}
