using Healin.Domain.Enums;
using System;

namespace Healin.Application.Requests
{
    public class VaccineDoseRequest : RequestBase
    {
        public Guid Id { get; set; }
        public Guid VaccineId { get; set; }
        public DoseType DoseType { get; set; }
        public DateTime DoseDate { get; set; }
    }
}
