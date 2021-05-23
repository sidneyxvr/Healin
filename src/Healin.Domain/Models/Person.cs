using Healin.Domain.Enums;
using Healin.Domain.ValueObjects;
using Healin.Shared.Data;
using System;

namespace Healin.Domain.Models
{
    public abstract class Person : AuditableEntity
    {
        public string Name { get; protected set; }
        public Cpf Cpf { get; protected set; }
        public Email Email { get; protected set; }
        public Gender Gender { get; protected set; }
        public Phone Phone { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public FilePath ImagePath { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public Address Address { get; protected set; }
    }
}
