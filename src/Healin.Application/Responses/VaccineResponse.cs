using System;

namespace Healin.Application.Responses
{
    public class VaccineResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
