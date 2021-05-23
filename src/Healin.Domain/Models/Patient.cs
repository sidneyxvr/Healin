using Healin.Domain.Enums;
using Healin.Domain.ValueObjects;
using Healin.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Healin.Domain.Models
{
    public class Patient : Person
    {
        public string SusNumber { get; private set; }

        //EF Realtional
        public ICollection<ExamResult> Exams { get; private set; } = new List<ExamResult>();
        public ICollection<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
        public ICollection<Doctor> Doctors { get; private set; } = new List<Doctor>();

        protected Patient() { }

        public static Patient New(Guid id, string name, string cpf, string email, Gender gender, string phone, DateTime birthDate, string susNumber)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 50, "O nome deve ter no máximo 50 caracteres");

            AssertionConcern.AssertIsEnum(gender, typeof(Gender), "Sexo inválido");

            AssertionConcern.AssertArgumentRange(birthDate, DateTime.Now.AddYears(-100), DateTime.Now, "Data de Nascimento inválida");

            return new Patient
            {
                Id = id,
                Name = name.ToTitleCase(),
                Cpf = cpf,
                Email = email,
                Gender = gender,
                Phone = phone,
                BirthDate = birthDate,
                SusNumber = susNumber,
                IsDeleted = false,
                IsActive = true,
            };
        }

        public void Update(string name, string cpf, DateTime birthDate, Gender gender, string phone, string susNumber)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 50, "O nome deve ter no máximo 50 caracteres");

            AssertionConcern.AssertIsEnum(gender, typeof(Gender), "Sexo inválido");

            AssertionConcern.AssertArgumentRange(birthDate, DateTime.Now.AddYears(-100), DateTime.Now, "Data de Nascimento inválida");

            Name = name.ToTitleCase();
            Cpf = cpf;
            Gender = gender;
            Phone = phone;
            BirthDate = birthDate;
            SusNumber = susNumber?.Trim();
        }

        public void UpdateAddress(string street, string number, string district, string city, string state, string postalCode, string complement) 
            => Address = new Address(street, number, district, city, state, postalCode, complement);

        public void Delete() => IsDeleted = true;

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        public void AddImagePath(string imagePath) => ImagePath = FilePath.CreateImage(imagePath);

        public void DeleteImage() => ImagePath = null;

        public void AddDoctor(Doctor doctor)
        {
            if (Doctors.Any(d => d.Id == doctor.Id))
            {
                return;
            }

            Doctors.Add(doctor);
        }

        public void RemoveDoctor(Doctor doctor)
        {
            if (Doctors is null || Doctors.All(d => d.Id != doctor.Id))
            {
                return;
            }

            var doctorToAdd = Doctors.FirstOrDefault(d => d.Id == doctor.Id);

            Doctors.Remove(doctorToAdd);
        }
    }
}
