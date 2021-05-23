using Healin.Domain.Enums;
using System;

namespace Healin.Application.Responses
{
    public class PatientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string SusNumber { get; set; }
        public string GenderDescription { get; set; }
        public string ImagePath { get; set; }
    }
}
