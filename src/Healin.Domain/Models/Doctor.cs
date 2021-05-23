using Healin.Domain.Enums;
using Healin.Domain.ValueObjects;
using Healin.Shared.Utils;
using System;
using System.Collections.Generic;

namespace Healin.Domain.Models
{
    public class Doctor : Person
    {
        public string Crm { get; private set; }

        //EF Realtional
        public ICollection<Specialty> Specialties { get; private set; }
        public ICollection<Patient> Patients { get; private set; }

        protected Doctor() { }

        public static Doctor New(Guid id, string name, string cpf, string email, 
            Gender gender, string phone, DateTime birthDate, string crm)
        {
            ValidateProperties(name, gender, birthDate, crm);

            return new Doctor
            {
                Id = id,
                Name = name?.Trim()?.ToTitleCase(),
                Cpf = cpf,
                Email = email,
                Gender = gender,
                Phone = phone,
                BirthDate = birthDate,
                Crm = crm,
                IsDeleted = false,
                IsActive = true
            };
        }

        public void Update(string name, string cpf, Gender gender, 
            string phone, DateTime birthDate, string crm)
        {
            ValidateProperties(name, gender, birthDate, crm);

            Name = name?.Trim()?.ToTitleCase();
            Cpf = cpf;
            Gender = gender;
            Phone = phone;
            BirthDate = birthDate;
            Crm = crm;
        }

        public void UpdateAddress(string street, string number, string district, string city, string state, string postalCode, string complement)
            => Address = new Address(street, number, district, city, state, postalCode, complement);

        public void Delete() => IsDeleted = true;

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        public void AddImagePath(string imagePath) => ImagePath = FilePath.CreateImage(imagePath);
        
        public void RemoveImagePath() => ImagePath = null;

        private static void ValidateProperties(string name, Gender gender, DateTime birthDate, string crm)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name.Trim(), 50, "O nome deve ter no máximo 50 caracteres");

            AssertionConcern.AssertIsEnum(gender, typeof(Gender), "Sexo inválido");

            AssertionConcern.AssertArgumentRange(birthDate, DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18), "Data de Nascimento inválida");

            AssertionConcern.AssertArgumentNotEmpty(crm, "O crm é obrigatório");
            AssertionConcern.AssertArgumentLength(crm, 10, "O crm deve ter no máximo 10 caracteres");
        }
    }
}
