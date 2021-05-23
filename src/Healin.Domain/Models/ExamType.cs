using Healin.Shared.Data;
using Healin.Shared.Utils;
using System;
using System.Collections.Generic;

namespace Healin.Domain.Models
{
    public class ExamType : AuditableEntity
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public Guid ExamId { get; private set; }

        //EF Relational
        public ICollection<ExamResult> ExamResults { get; private set; }
        public Exam Exam { get; private set; }
        protected ExamType() { }

        public static ExamType New(string name, Guid examId)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 100, "O nome deve ter no máximo 100 caracteres");

            AssertionConcern.AssertArgumentNotEmpty(examId, "O exame é obrigatório");

            return new ExamType
            {
                Name = name,
                ExamId = examId,
                IsActive = true,
                IsDeleted = false
            };

        }

        public void Update(string name, Guid examId)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "O nome é obrigatório");
            AssertionConcern.AssertArgumentLength(name, 100, "O nome deve ter no máximo 100 caracteres");

            AssertionConcern.AssertArgumentNotEmpty(examId, "O exame é obrigatório");

            Name = name;
            ExamId = examId;
        }

        public void Delete() => IsDeleted = true;

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;
    }
}
