using Healin.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace Healin.Application.Requests
{
    public class PatientRequest : RequestBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public IFormFile ImageUpload { get; set; }
        public string Password { get; set; }
        public string SusNumber { get; set; }
    }
}
