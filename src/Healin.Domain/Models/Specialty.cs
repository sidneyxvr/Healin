using Healin.Shared.Data;
using Healin.Shared.Utils;
using System.Collections.Generic;

namespace Healin.Domain.Models
{
    public class Specialty : AuditableEntity
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public ICollection<Doctor> Doctors { get; private set; }

        protected Specialty() { }

        public static Specialty New(string name)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 50, "O nome deve ter no máximo 50 caracteres");

            return new Specialty
            {
                Name = name,
                IsActive = true,
                IsDeleted = false,
            };
        }

        public void Update(string name)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 50, "O nome deve ter no máximo 50 caracteres");

            Name = name;
        }

        public void Delete() => IsDeleted = true;

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;
    }
}
